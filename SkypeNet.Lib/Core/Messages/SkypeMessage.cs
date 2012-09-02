using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SkypeNet.Lib.Core.Messages
{
    public class SkypeMessage
    {
        /// <summary>
        /// The original raw response string that was received from the skype applicaton
        /// this is useful for debugging parsing and unexpected problems.
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// The data portion for the skype message
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// The error code returned from skype, 0 indicates no error
        /// </summary>
        public int Error { get; set; }

        /// <summary>
        /// The string representation of the error returned from Skype
        /// along with extra information if it is available
        /// </summary>
        public string ErrorMessage { get; set; }
    }

    /*
    public abstract class SkypeMessage
    {
        /// <summary>
        /// Static constructor for the type reflects all derived types and creates a 
        /// dictionary containing their properties and command types for quicker access
        /// during execution
        /// </summary>
        static SkypeMessage()
        {
            // Attribute types
            
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                
            }

            var messageTypes = types.Where(x => x.GetCustomAttributes(typeof(SkypeCommandAttribute), false).Length > 0).ToArray();
            foreach (var msgType in messageTypes)
            {
                var aCommand = (SkypeCommandAttribute)msgType.GetCustomAttributes(typeof(SkypeCommandAttribute), false)[0];
                
            }

        }


        /// <summary>
        /// The original raw response string that was received from the skype applicaton
        /// this is useful for debugging parsing and unexpected problems.
        /// </summary>
        public string Response { get; protected set; }

        //<- ERROR 7 GET: invalid WHAT
        /// <summary>
        /// The error code returned from skype, 0 indicates no error
        /// </summary>
        public int Error { get; protected set; }

        /// <summary>
        /// The string representation of the error returned from Skype
        /// along with extra information if it is available
        /// </summary>
        public string ErrorMessage { get; protected set; }

        /// <summary>
        /// Forces the 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected internal bool ParseResponse( string response )
        {
            
        }

        /// <summary>
        /// Returns true if the message has been completely parsed according to the specified protocol version
        /// </summary>
        /// <param name="protocolVersion">ignored for now! the protocol version that the client supports. 
        /// Default is <see cref="SkypeApiVersions.All"/></param>
        /// <returns>true if the message is complete and is not expecting any more parsing data, false if 
        /// more parsing is expected</returns>
        protected internal bool IsComplete(SkypeApiVersions protocolVersion = SkypeApiVersions.All)
        {
            // Here we need to check which properties that have the SkypeReply attribute set have been populated for the message
        }
        
        public static SkypeMessage CreateFromRespose(string response)
        {
            if( string.IsNullOrWhiteSpace(response) )
                throw new InvalidOperationException("Response from skype is null, this is a developer error! Contact support.");

            var parts = response.Split(' ');
            if( parts.Length <= 0 )
                throw new InvalidOperationException("Response from skype contains only empty values, this is a developer error! Contact support.");

            // Check if the response contains an error
            if (0 == string.Compare(parts[0], "ERROR", StringComparison.OrdinalIgnoreCase))
                return new UnknownMessage(response) {Response = response, Error = int.Parse(parts[1]), ErrorMessage = string.Join(" ", parts, 2, parts.Length-2)};
            
            return new UnknownMessage(response) { Response = response};
        }


        /// <summary>
        /// Generates the command text for the message
        /// </summary>
        /// <returns></returns>
        public string GetCommand()
        {
        
        }
    }*/
}
