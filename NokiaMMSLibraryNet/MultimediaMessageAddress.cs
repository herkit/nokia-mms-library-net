using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NokiaMMSLibraryNet
{
    public class MultimediaMessageAddress
    {

        private string address;
        private byte type;  

        /**  
           * Creates a new and empty MM address  
           */
        public MultimediaMessageAddress()
        {
            address = null;
            type = 0;
        }

        /**  
         * Creates a MM address specifying the address and the type.  
         *  
         * param addr the string representing the address  
         * param type the type of the address. It can be:  
         * ADDRESS_TYPE_UNKNOWN, ADDRESS_TYPE_PLMN,ADDRESS_TYPE_IPV4,  
         * ADDRESS_TYPE_IPV6, ADDRESS_TYPE_EMAIL   
         *  
         */
        public MultimediaMessageAddress(string addr, byte type)
        {
            setAddress(addr, type);
        }

        /**  
         * Creates a new MM address initialising it to the value passed as parameter.  
         */
        public MultimediaMessageAddress(MultimediaMessageAddress value)
        {
            setAddress(value.address, value.type);
        }

        /**  
         * Sets MM address value specifying the address and the type.  
         *  
         * param addr the string representing the address  
         * param type the type of the address. It can be:  
         * ADDRESS_TYPE_UNKNOWN, ADDRESS_TYPE_PLMN,ADDRESS_TYPE_IPV4,  
         * ADDRESS_TYPE_IPV6, ADDRESS_TYPE_EMAIL   
         *  
         */
        public void setAddress(string addr, byte type)
        {
            if (addr != null)
            {
                this.address = addr;
                this.type = type;
            }
        }

        /**  
         * Retrieves the MM address value.  
         *  
         */
        public string Address
        {
            get {
                return address;
            }
        }

        /**  
         * Retrieves the MM address value in the full format. For example: +358990000066/TYPE=PLMN,  
         * joe@user.org, 1.2.3.4/TYPE=IPv4  
         *  
         */
        public string FullAddress
        {
            get
            {
                switch (type)
                {
                    case MultimediaMessageConstants.ADDRESS_TYPE_PLMN: return address + "/TYPE=PLMN";
                    case MultimediaMessageConstants.ADDRESS_TYPE_IPV4: return address + "/TYPE=IPv4";
                    case MultimediaMessageConstants.ADDRESS_TYPE_IPV6: return address + "/TYPE=IPv6";
                    default: return address;
                }
            }
        }

        /**  
         * Retrieves the MM address type.  
         *  
         * return the type of the address. It can be:  
         * ADDRESS_TYPE_UNKNOWN, ADDRESS_TYPE_PLMN,ADDRESS_TYPE_IPV4,  
         * ADDRESS_TYPE_IPV6, ADDRESS_TYPE_EMAIL   
         *  
         */
        public byte Type
        {
            get
            {
                return type;
            }
        }
    }
}
