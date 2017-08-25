using System;
using System.Linq;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut {
    public class PeanutEditOptions {
        private readonly Core.Domain.Peanuts.Peanut _peanut;
        private readonly User _currentUser;

        public PeanutEditOptions(Core.Domain.Peanuts.Peanut peanut, User currentUser) {
            Require.NotNull(peanut, "peanut");
            Require.NotNull(currentUser, "currentUser");

            _peanut = peanut;
            _currentUser = currentUser;

            bool isCurrentUserConfirmedParticipator =
                    peanut.Participations.Any(
                        p => p.UserGroupMembership.User.Equals(currentUser) && p.ParticipationState == PeanutParticipationState.Confirmed);

            bool isCurrentUserParticipator =
                    peanut.Participations.Any(
                        p => p.UserGroupMembership.User.Equals(currentUser));

            /*Man kann die Teilnahme hinzufügen, wenn man noch kein Teilnehmer ist und der Peanut noch nicht fixiert ist*/
            CanAddParticipation = !isCurrentUserConfirmedParticipator && !peanut.IsFixed;

            /*Man kann die Teilnahme absagen, wenn man Teilnehmer ist und der Peanut noch nicht fixiert ist*/
            CanCancelParticipation = isCurrentUserParticipator && !peanut.IsFixed;

            /*Man kann den Peanut abrechnen, wenn dieser fixiert ist. Tendenziell können zu einem Peanut mehrere Rechnungen erstellt werden.*/
            CanBillPeanut = (peanut.PeanutState == PeanutState.Realized || peanut.PeanutState == PeanutState.Assembling || peanut.PeanutState == PeanutState.Started) && isCurrentUserConfirmedParticipator;
            
            /*Man kann einen Peanut bearbeiten, wenn man Teilnehmer ist und der Peanut noch nicht fixiert ist*/
            CanUpdatePeanut = !peanut.IsFixed && isCurrentUserConfirmedParticipator;

            /*Man kann den Status eines Peanut bearbeiten, wenn man Teilnehmer ist*/
            CanUpdatePeanutState = isCurrentUserConfirmedParticipator;
            
        }

        /// <summary>
        ///     Ruft ab, ob sich der Nutzer als neuer/weiterer Teilnehmer eintragen kann.
        /// </summary>
        public bool CanAddParticipation { get; private set; }

        /// <summary>
        ///     Ruft ab, ob der Nutzer den Peanut abrechnen kann.
        /// </summary>
        public bool CanBillPeanut { get; private set; }

        /// <summary>
        ///     Ruft ab, ob sich der Nutzer als neuer/weiterer Teilnehmer eintragen kann.
        /// </summary>
        public bool CanCancelParticipation { get; private set; }

        /// <summary>
        ///     Ruft ab, ob der Nutzer den Peanut bearbeiten kann.
        /// </summary>
        public bool CanUpdatePeanut { get; private set; }

        /// <summary>
        ///     Ruft ab, ob der Nutzer den Status des Peanut bearbeiten kann.
        /// </summary>
        public bool CanUpdatePeanutState {
            get; private set;
        }

        public bool CanUpdateStateTo(PeanutState peanutState) {
            if (!CanUpdatePeanutState) {
                /*Wenn der Status generell nicht geändert werden kann, dann geht sowieso Nichts.*/
                return false;
            }

            switch (peanutState) {
                case PeanutState.Scheduling:
                    /*Wenn die Planung als abgeschlossen markiert wurde, aber noch Änderungen notwendig sind*/
                    /*Wenn versehentlich auf Beschaffung läuft geklickt wurde*/
                    /*Es darf noch keine Rechnung erstellt wurden sein*/
                    return (_peanut.PeanutState == PeanutState.PurchasingStarted || _peanut.PeanutState == PeanutState.SchedulingDone || _peanut.PeanutState == PeanutState.Canceled) && !_peanut.IsCleared;
                case PeanutState.SchedulingDone:
                    /*Wenn der Peanut in der Planungsphase ist, kann der Status auf Beschaffung läuft gesetzt werden => Standardfall*/
                    /*Wenn die Beschaffung (aus Versehen) gestartet wurde, kann diese abgebrochen werden*/
                    return _peanut.PeanutState == PeanutState.Scheduling || _peanut.PeanutState == PeanutState.PurchasingStarted;
                case PeanutState.PurchasingStarted:
                    /*Wenn der Peanut in der Planungsphase ist, kann der Status auf Beschaffung läuft gesetzt werden => Standardfall*/
                    /*Wenn der Peanut in der Phase "Beschaffung abgeschlossen ist, kann der Status auf Beschaffung läuft gesetzt werden => Irgendwas wurde vergessen"*/
                    /*Es darf noch keine Rechnung erstellt wurden sein*/
                    return _peanut.PeanutState == PeanutState.Scheduling || _peanut.PeanutState == PeanutState.SchedulingDone || _peanut.PeanutState == PeanutState.PurchasingDone && !_peanut.IsCleared;
                case PeanutState.PurchasingDone:
                    /*Wenn der Peanut in der Phase "Beschaffung läuft ist, kann der Status auf Beschaffung abgeschlossen gesetzt werden => Standardfall"*/
                    /*Wenn der Peanut in der Planungsphase ist, kann der Status auf Beschaffung abgeschlossen gesetzt werden => Abkürzung wenn vergessen auf Beschaffung läuft zu setzen*/
                    /*Wenn der Peanut in der Phase "Herstellung" ist, kann der Status auf Beschaffung abgeschlossen gesetzt werden => Herstellung wurde abgebrochen/verschoben*/
                    /*Es darf noch keine Rechnung erstellt wurden sein*/
                    return _peanut.PeanutState == PeanutState.Scheduling || _peanut.PeanutState == PeanutState.SchedulingDone || _peanut.PeanutState == PeanutState.PurchasingStarted || _peanut.PeanutState == PeanutState.Assembling && !_peanut.IsCleared;
                case PeanutState.Assembling:
                    /*Wenn der Peanut in der Phase "Beschaffung abgeschlossen ist, kann der Status auf Beschaffung abgeschlossen gesetzt werden => Standardfall"*/
                    /*Wenn der Peanut in der Phase "Beschaffung läuft" ist, kann der Status auf "Herstellung" gesetzt werden => Abkürzung wenn vergessen auf Beschaffung abgeschlossen zu setzen*/
                    /*Wenn der Peanut in der Planungsphase ist, kann der Status auf "Herstellung" gesetzt werden => Abkürzung wenn keine Beschaffung notwendig ist*/
                    /*Wenn der Peanut in der Phase "Started" ist, kann der Status auf "Herstellung" gesetzt werden => Start wurde abgebrochen/verschoben*/
                    return _peanut.PeanutState == PeanutState.Scheduling || _peanut.PeanutState == PeanutState.SchedulingDone || _peanut.PeanutState == PeanutState.PurchasingStarted || _peanut.PeanutState == PeanutState.PurchasingDone;
                case PeanutState.Started:
                    /*Wenn der Peanut in der Phase "Beschaffung abgeschlossen ist, kann der Status auf Beschaffung abgeschlossen gesetzt werden => Standardfall"*/
                    /*Wenn der Peanut in der Phase "Beschaffung läuft" ist, kann der Status auf "Herstellung" gesetzt werden => Abkürzung wenn vergessen auf Beschaffung abgeschlossen zu setzen*/
                    /*Wenn der Peanut in der Planungsphase ist, kann der Status auf "Herstellung" gesetzt werden => Abkürzung wenn keine Beschaffung notwendig ist*/
                    /*Wenn der Peanut in der Phase "Started" ist, kann der Status auf "Herstellung" gesetzt werden => Start wurde abgebrochen/verschoben*/
                    return _peanut.PeanutState == PeanutState.Scheduling || _peanut.PeanutState == PeanutState.SchedulingDone || _peanut.PeanutState == PeanutState.PurchasingDone || _peanut.PeanutState == PeanutState.Assembling;
                case PeanutState.Realized:
                    /*Wenn der Peanut in der Phase "Started" ist, kann der Status auf "Durchgeführt" gesetzt werden => Standardfall*/
                    /*Wenn der Peanut in der Phase "Herstellung" ist, kann der Status auf Beschaffung abgeschlossen gesetzt werden => Abkürzung wenn Vergessen auf Started zu setzen*/
                    return _peanut.PeanutState == PeanutState.Scheduling || _peanut.PeanutState == PeanutState.SchedulingDone || _peanut.PeanutState == PeanutState.Started || _peanut.PeanutState == PeanutState.Assembling;
                case PeanutState.Canceled:
                    /*Wenn der Peanut noch nicht abgerechnet ist und die Beschaffung noch nicht abgeschlossen ist oder mit der Herstellung begonnen wurde, kann der Peanut abgesagt werden.*/
                    return !_peanut.IsCleared && !(_peanut.PeanutState == PeanutState.Started || _peanut.PeanutState == PeanutState.Assembling || _peanut.PeanutState == PeanutState.Realized || _peanut.PeanutState == PeanutState.PurchasingDone);
                default:
                    return false;
            }
        }
    }
}