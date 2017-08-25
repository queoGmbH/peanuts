using System;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Dto;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;

namespace Com.QueoFlow.Peanuts.Net.Core.TestDataBuilders {
    public class PaymentBuilder : Builder<Payment> {
        private readonly DateTime _createdAt = new DateTime(2017, 01, 01, 15, 30, 00);
        private readonly string _text = "Test Payment";
        private double _amount = 1;
        private User _createdBy;
        private PaymentType _paymentType;
        private UserGroupMembership _recipientUserGroupMembership;
        private User _requestRecipent;
        private User _requestSender;
        private UserGroupMembership _senderUserGroupMembership;

        public override Payment Build() {
            EntityCreatedDto entityCreatedDto = new EntityCreatedDto(_createdBy, _createdAt);
            PaymentDto paymentDto = new PaymentDto() { Amount = _amount, PaymentType = _paymentType, Text = _text };
            if (_recipientUserGroupMembership != null && _senderUserGroupMembership != null) {
                if (!_recipientUserGroupMembership.UserGroup.Equals(_senderUserGroupMembership.UserGroup)) {
                    throw new InvalidOperationException(
                        $"Der Empfänger {_recipientUserGroupMembership.User.DisplayName} muss in der gleichen Gruppe {_senderUserGroupMembership.UserGroup.Name} wie der Sender {_senderUserGroupMembership.User.DisplayName} sein");
                }
            }

            UserGroup userGroup = Create.A.UserGroup();

            if (_recipientUserGroupMembership == null) {
                _recipientUserGroupMembership = Create.A.UserGroupMembership().InUserGroup(userGroup);
            }
            if (_senderUserGroupMembership == null) {
                _senderUserGroupMembership = Create.A.UserGroupMembership().InUserGroup(userGroup);
            }

            if (_requestRecipent == null) {
                _requestRecipent = Create.A.User();
            }

            if (_requestSender == null) {
                _requestSender = Create.A.User();
            }

            return new Payment(paymentDto,
                _recipientUserGroupMembership.Account,
                _senderUserGroupMembership.Account,
                _requestRecipent,
                _requestSender,
                entityCreatedDto);
        }

        public PaymentBuilder FromSender(UserGroupMembership senderMembership) {
            _senderUserGroupMembership = senderMembership;
            return this;
        }

        public PaymentBuilder ToRecipient(UserGroupMembership recipientMembership) {
            _recipientUserGroupMembership = recipientMembership;
            return this;
        }

        public PaymentBuilder WithAmount(double amount) {
            _amount = amount;
            return this;
        }
    }
}