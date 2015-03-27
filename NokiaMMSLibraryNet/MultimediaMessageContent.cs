using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NokiaMMSLibraryNet
{
    public class MultimediaMessageContent
    {
        private string _contentLocation;
        private string m_sType = "";
        private string m_sContentId = "";
        private MemoryStream m_byteArray;
        private int m_iLength = 0;

        private bool flagContentLocationAvailable = false;

        /// <summary>
        /// Sets the type of the entry. 
        /// param value it's a valid content types. See WAP-203-WSP (WAP Forum) 
        /// Examples are: text/plain, image/jpeg, image/gif, etc. 
        ///      You can use also some constants like: 
        ///      CT_TEXT_HTML, CT_TEXT_PLAIN, CT_TEXT_WML, CT_IMAGE_GIF, 
        ///      CT_IMAGE_JPEG, CT_IMAGE_WBMP, CT_APPLICATION_SMIL, etc. 
        ///      (these constants are defined in the interface IMMConstants) 
        ///      
        /// Retrieves the type of the entry. Examples are: text/plain, image/jpeg, image/gif, etc. 
        /// 
        /// return the type of the entry. 
        /// </summary>
        public string Type
        {
            set
            {
                m_sType = value;
            }
            get
            {
                return m_sType;
            }
        }

        /// <summary>
        /// Sets the ID of the entry. 
        /// 
        /// Retrieves the ID of the entry. 
        /// 
        /// return the ID 
        /// </summary>
        public string ContentId
        {
            get
            {
                return m_sContentId;
            }
            set
            {
                m_sContentId = value;
            }
        }

        public string ContentLocation
        {
            get
            {
                return _contentLocation;
            }
            set
            {
                _contentLocation = value;
                flagContentLocationAvailable = true;
            }
        }

        public bool IsContentLocationAvailable
        {
            get { return flagContentLocationAvailable; }
        }

        /** 
         * 
         * 
         * return the length 
         */

        /// <summary>
        /// Retrieves the length in bytes of the entry. 
        /// </summary>
        /// <value>
        /// </value>
        public int Length
        {
            get
            {
                return m_iLength;
            }
        }

        /** 
         * Writes <code>len</code> bytes from the specified byte array starting at offset off. 
         * 
         * param buf the data 
         * param off the start offset in the data. 
         * param len the number of bytes to write. 
         */
        public void SetContent(byte[] buf, int off, int len)
        {
            m_iLength = len;
            m_byteArray = new MemoryStream(len);
            m_byteArray.Write(buf, off, len);
        }

        /** 
         * Retrieves the array of byte representing the entry. 
         * 
         * return the array of byte 
         */
        public byte[] GetContent()
        {
            return m_byteArray.ToArray();
        }

        /** 
         * Retrieves the String representing the entry. 
         * 
         * return buffer as a String object 
         */
        public String GetContentAsString()
        {
            return m_byteArray.ToString();
        }

        /** 
         * Stores the entry into a file. 
         * 
         * param filename the name of the file. 
         */
        public void SaveToFile(String filename)
        {
            FileStream f = null;

            try
            {
                f = System.IO.File.Create(filename);
                m_byteArray.WriteTo(f);
            }
            catch (FileNotFoundException ioErr)
            {
                Console.Error.WriteLine(ioErr);
                throw new IOException();
            }

        }

        /** 
      * Creates the object representing the content. 
      */
        public MultimediaMessageContent()
        {
        }
    }

}
