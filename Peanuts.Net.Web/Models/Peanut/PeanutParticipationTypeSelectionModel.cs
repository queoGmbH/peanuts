using System;
using System.Collections;
using System.Collections.Generic;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut
{
    public class PeanutParticipationTypeSelectionModel
    {
        public PeanutParticipationTypeSelectionModel()
        {
            SelectableParticipationTypes = new List<PeanutParticipationType>();
        }

        public PeanutParticipationTypeSelectionModel(PeanutParticipationType selectedParticipationType, IList<PeanutParticipationType> selectableParticipationTypes)
        {
            SelectableParticipationTypes = selectableParticipationTypes;
            SelectedParticipationType = selectedParticipationType;
        }

        public PeanutParticipationType SelectedParticipationType { get; set; }

        public IList<PeanutParticipationType> SelectableParticipationTypes { get; set; }
    }
}