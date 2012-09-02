using System.ComponentModel;

namespace SkypeNet.Lib.Core
{
    /// <summary>
    /// The types of skype commands
    /// </summary>
    public enum SkypeCommandTypes
    {
        /// <summary>
        /// No directional command type value
        /// </summary>
        [Description("")]
        None,

        /// <summary>
        /// The GET command
        /// </summary>
        [Description("GET")]
        Get,

        /// <summary>
        /// The SET command
        /// </summary>
        [Description("SET")]
        Set,

        /// <summary>
        /// The ALTER command, modifying something that is
        /// ongoing in the application
        /// </summary>
        [Description("ALTER")]
        Alter
    }

    public enum SkypeSmsTypes
    {
        /// <summary>
        /// normal outbound SMS.
        /// </summary>
        OUTGOING,

        /// <summary>
        /// Refer to [#SMS_NUMBER_VALIDATION SMS reply-to validation] for more information.
        /// </summary>
        CONFIRMATION_CODE_REQUEST,

        /// <summary>
        /// Refer to [#SMS_NUMBER_VALIDATION SMS reply-to validation] for more information.
        /// </summary>
        CONFIRMATION_CODE_SUBMIT
    }
}
