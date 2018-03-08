using System;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace Com.QueoFlow.Peanuts.Net.Core.Persistence.NHibernate.UserTypes {
    public class VersionUserType : IUserType {
        /// <inheritdoc />
        public bool Equals(object x, object y) {
            return object.Equals(x, y);
        }

        /// <inheritdoc />
        public int GetHashCode(object x) {
            if (x != null) {
                return x.GetHashCode();
            } else {
                return 0;
            }
        }

        /// <inheritdoc />
        public object NullSafeGet(IDataReader rs, string[] names, object owner) {
            object obj = NHibernateUtil.String.NullSafeGet(rs, names[0]);

            if (obj == null) {
                return null;
            }

            Version version;
            if (Version.TryParse(obj.ToString(), out version)) {
                return version;
            } else {
                return null;
            }
        }

        /// <inheritdoc />
        public void NullSafeSet(IDbCommand cmd, object value, int index) {
            if (value is Version) {
                ((IDataParameter)cmd.Parameters[index]).Value = ((Version)value).ToString();
            } else {
                ((IDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
            }
        }

        /// <inheritdoc />
        public object DeepCopy(object value) {
            if (value != null) {
                return Version.Parse(value.ToString());
            } else {
                return null;
            }
        }

        /// <inheritdoc />
        public object Replace(object original, object target, object owner) {
            return original;
        }

        /// <inheritdoc />
        public object Assemble(object cached, object owner) {
            return cached;
        }

        /// <inheritdoc />
        public object Disassemble(object value) {
            return value;
        }

        /// <inheritdoc />
        public SqlType[] SqlTypes {
            get {
                return new [] { new SqlType(DbType.String) };
            } 
        }

        /// <summary>
        /// The type returned by <c>NullSafeGet()</c>
        /// </summary>
        public Type ReturnedType {
            get {
                return typeof(Version);
            } 
        }

        /// <summary>Are objects of this type mutable?</summary>
        public bool IsMutable {
            get { return false; }
        }
    }

}