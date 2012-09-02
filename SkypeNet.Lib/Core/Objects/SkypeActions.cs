namespace SkypeNet.Lib.Core
{
    /// <summary>
    /// Most call properties are read-only. The following properties are read-write and can be modified with the SET command:
    /// </summary>
    public enum SkypeCallSetProperties
    {
        /// <summary>
        ///  – for call control. Possible values:
        /// ONHOLD – hold call
        /// INPROGRESS – answer or resume call
        /// FINISHED – hang up call
        /// </summary>
        //[SkypeProperty("STATUS", new{"ONHOLD", "INPROGRESS", "FINISHED"})]
        STATUS,

        /// <summary>
        ///  – sets call as seen, so that a missed call is seen and can be removed from the missed calls list.
        /// </summary>
        SEEN,
        
        /// <summary>
        ///  – sends VALUE as DTMF. Permitted symbols in VALUE are: {0..9,#,*}.
        /// </summary>
        DTMF,
        
        /// <summary>
        ///  – joins call with another call into conference. VALUE is the ID of another call.
        /// </summary>
        JOIN_CONFERENCE
    }

    /*public enum SkypeSetActions
    {
        
    }

    public enum SkypeAlterActions
    {

    }

    public enum SkypeSearchActions
    {

    }

    public enum SkypeOpenActions
    {

    }*/

}
