using Com.QueoFlow.Peanuts.Net.Core.Domain.Peanuts;

using FluentNHibernate.Mapping;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.Mappings {
    public class PeanutMap : EntityMap<Peanut> {
        protected PeanutMap() {
            /*Allgemeine Informationen*/
            Map(user => user.Name).Not.Nullable().Length(255);
            Map(user => user.Description).Nullable().Length(4000);
            Map(user => user.Day).Not.Nullable().CustomSqlType("date");
            References(peanut => peanut.UserGroup).Not.Nullable().ForeignKey("FK_PEANUT_USERGROUP");

            Map(user => user.PeanutState).Not.Nullable();


            HasManyToMany(peanut => peanut.Documents)
                    .Table("tblPeanutDocuments")
                    .Access.CamelCaseField(Prefix.Underscore)
                    .ForeignKeyConstraintNames("FK_PEANUT_WITH_DOCUMENTS", "FK_DOCUMENT_OF_PEANUT");
            HasMany(peanut => peanut.Participations)
                    .Table("tblPeanutParticipation")
                    .Access.CamelCaseField(Prefix.Underscore)
                    .ForeignKeyConstraintName("FK_PEANUT_PARTICIPATION")
                    .Cascade.AllDeleteOrphan()
                    .Inverse();

            HasMany(peanut => peanut.Requirements)
                    .Table("tblPeanutRequirement")
                    .Access.CamelCaseField(Prefix.Underscore)
                    .KeyColumn("Peanut_Id")
                    .ForeignKeyConstraintName("FK_PEANUT_REQUIREMENT")
                    .Component(comp => {
                        comp.Map(user => user.Name).Not.Nullable().Length(100);
                        comp.Map(user => user.Quantity).Not.Nullable();
                        comp.Map(user => user.Unit).Not.Nullable().Length(15);
                        comp.Map(user => user.Url).Nullable().Length(255);
                    })
                    .Cascade.AllDeleteOrphan();

            References(peanut => peanut.CreatedBy).Not.Nullable().ForeignKey("FK_CREATOR_OF_PEANUT");
            Map(peanut => peanut.CreatedAt).Not.Nullable();
            References(peanut => peanut.ChangedBy).Nullable().NotFound.Ignore();
            Map(peanut => peanut.ChangedAt).Nullable();

            HasManyToMany(peanut => peanut.Bills)
                    .Table("tblBillForPeanut")
                    .Access.CamelCaseField(Prefix.Underscore)
                    .ForeignKeyConstraintNames("FK_PEANUT_WITH_BILLS", "FK_BILL_FOR_PEANUT");

            HasMany(peanut => peanut.Comments)
                    .Table("tblPeanutComment")
                    .Access.CamelCaseField(Prefix.Underscore)
                    .KeyColumn("Peanut_Id")
                    .Component(comp => {
                            comp.Map(peanutComment => peanutComment.Comment).Length(1000);
                            comp.References(peanut => peanut.CreatedBy).Not.Nullable().ForeignKey("FK_CREATOR_OF_PEANUTCOMMENT");
                            comp.Map(peanut => peanut.CreatedAt).Not.Nullable();
                            comp.References(peanut => peanut.ChangedBy).Nullable().NotFound.Ignore();
                            comp.Map(peanut => peanut.ChangedAt).Nullable();
                        }
                    )
                    .Cascade.All()
                    .Not.KeyNullable()                 
                    .ForeignKeyConstraintName("FK_PEANUT_WITH_COMMENTS");
        }
    }
}