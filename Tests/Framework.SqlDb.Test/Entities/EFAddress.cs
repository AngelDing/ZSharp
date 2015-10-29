using System;
using System.Collections.Generic;
using ZSharp.Framework.ValueObjects;

namespace Framework.SqlDb.Test
{
    [Serializable]
    public class EFAddress : BaseValueObject
    {
        private string country;
        private string state;
        private string city;
        private string street;
        private string zip;

        public static readonly EFAddress Emtpy = new EFAddress(null, null, null, null, null);

        public EFAddress() { }

        public EFAddress(string country, string state, string city, string street, string zip)
        {
            this.country = country;
            this.state = state;
            this.city = city;
            this.street = street;
            this.zip = zip;
        }

        public string Country
        {
            get { return country; }
            set { this.country = value; }
        }

        public string State
        {
            get { return state; }
            set { this.state = value; }
        }

        public string City
        {
            get { return city; }
            set { this.city = value; }
        }

        public string Street
        {
            get { return street; }
            set { this.street = value; }
        }

        public string Zip
        {
            get { return zip; }
            set { this.zip = value; }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null)
                return false;
            if (!(obj is EFAddress))
                return false;
            EFAddress other = (EFAddress)obj;
            if (other == null)
                return false;
            return this.country.Equals(other.country) &&
                this.state.Equals(other.state) &&
                this.city.Equals(other.city) &&
                this.street.Equals(other.street) &&
                this.zip.Equals(other.zip);
        }

        public override int GetHashCode()
        {
            return this.country.GetHashCode() ^
                this.state.GetHashCode() ^
                this.city.GetHashCode() ^
                this.street.GetHashCode() ^
                this.zip.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} {1}, {2}, {3}, {4}", zip, street, city, state, country);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(EFAddress a, EFAddress b)
        {
            if ((object)a == null)
            {
                return (object)b == null;
            }
            return a.Equals(b);
        }

        public static bool operator !=(EFAddress a, EFAddress b)
        {
            return !(a == b);
        }
    }
}
