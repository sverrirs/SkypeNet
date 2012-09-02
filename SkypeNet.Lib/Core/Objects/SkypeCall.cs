using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkypeNet.Lib.Core.Objects
{
    public sealed class SkypeCall
    {
        /// <summary>
        /// The call id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///  – time when call was placed (UNIX timestamp), for example CALL 17 TIMESTAMP 1078958218
        /// </summary>
        [SkypeParameter("TIMESTAMP")]
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// for example CALL 17 PARTNER_HANDLE mike. In case of SkypeOut and SkypeIn calls this property contains the PSTN number of remote party, prefixed by countrycode (+123456789).
        /// </summary>
        [SkypeParameter("PARTNER_HANDLE")]
        public string PartnerHandle { get; set; }
        /// <summary>
        ///  – for example CALL 17 PARTNER_DISPNAME Mike Mann
        /// </summary>
        [SkypeParameter("PARTNER_DISPNAME")]
        public string PartnerDisplayName { get; set; }
        /// <summary>
        /// This property is set when you a) have a SkypeIn number and b) receive an incoming PSTN call. The value of call’s target identity property is then set to your own SkypeIn number. This property is not set if the incoming call is P2P. This property was introduced in API version 3.1
        /// </summary>
        [SkypeParameter("TARGET_IDENTITY")]
        public string TargetIdentity { get; set; }
        /// <summary>
        ///  – if the CONF_ID>0 the call is a conference call, for example: CALL 17 CONF_ID 0
        /// </summary>
        [SkypeParameter("CONF_ID")]
        public string ConferenceId { get; set; }
        /// <summary>
        ///  – call type, for example: CALL 17 TYPE OUTGOING_PSTN . Possible values:
        /// INCOMING_PSTN – incoming call from PSTN
        /// OUTGOING_PSTN – outgoing call to PSTN
        /// INCOMING_P2P – incoming call from P2P
        /// OUTGOING_P2P – outgoing call to P2P
        /// </summary>
        [SkypeParameter("TYPE")]
        public string Type { get; set; }
        /// <summary>
        ///  call status, for example: CALL 17 STATUS FAILED . Possible values:
        /// UNPLACED – call was never placed
        /// ROUTING – call is currently being routed
        /// EARLYMEDIA – with pstn it is possible that before a call is established, early media is played. For example it can be a calling tone or a waiting message such as all operators are busy.
        /// FAILED – call failed – try to get a FAILUREREASON for more information.
        /// RINGING – currently ringing
        /// INPROGRESS – call is in progress
        /// ONHOLD – call is placed on hold
        /// FINISHED – call is finished
        /// MISSED – call was missed
        /// REFUSED – call was refused
        /// BUSY – destination was busy
        /// CANCELLED (Protocol 2)
        /// TRANSFERRING – Refer to ALTER CALL TRANSFER command. Added in protocol 7 (API version 3.0)
        /// TRANSFERRED – Refer to ALTER CALL TRANSFER command. Added in protocol 7 (API version 3.0)
        /// VM_BUFFERING_GREETING – voicemail greeting is being downloaded
        /// VM_PLAYING_GREETING – voicemail greeting is being played
        /// VM_RECORDING – voicemail is being recorded
        /// VM_UPLOADING – voicemail recording is finished and uploaded into server
        /// VM_SENT – voicemail has successfully been sent
        /// VM_CANCELLED – leaving voicemail has been cancelled
        /// VM_FAILED – leaving voicemail failed; check FAILUREREASON
        /// WAITING_REDIAL_COMMAND – This status is set when your outgoing call to PSTN gets rejected by remote party. This state was added in version 3.5 (protocol 8).
        /// REDIAL_PENDING – This status is set when you press redial button on the Call Phones tab of the Skype interface. This state was added in version 3.5 (protocol 8).
        /// </summary>
        [SkypeParameter("STATUS")]
        public string Status { get; set; }
        /// <summary>
        ///  Commands ALTER CALL VIDEO_SEND and RECEIVE ALTER CALL VIDEO_ RECEIVE can be used to change call video status. 
        /// Possible values of this property are:
        /// VIDEO_NONE
        /// VIDEO_SEND_ENABLED
        /// VIDEO_RECV_ENABLED
        /// VIDEO_BOTH_ENABLED
        /// </summary>
        [SkypeParameter("VIDEO_STATUS")]
        public string VideoStatus { get; set; }
        /// <summary>
        /// – possible values of this property are:
        /// NOT_AVAILABLE – the client does not have video capability because video is disabled or a webcam is unplugged).
        /// AVAILABLE – the client is video-capable but the video is not running (can occur during a manual send).
        /// STARTING – the video is sending but is not yet running at full speed.
        /// REJECTED – the receiver rejects the video feed (can occur during a manual receive).
        /// RUNNING – the video is actively running.
        /// STOPPING – the active video is in the process of stopping but has not halted yet.
        /// PAUSED – the video call is placed on hold.
        /// </summary>
        [SkypeParameter("VIDEO_SEND_STATUS")]
        public string VideoSendStatus { get; set; }
        /// <summary>
        /// – possible values of this property are:
        /// NOT_AVAILABLE – the client does not have video capability because video is disabled or a webcam is unplugged).
        /// AVAILABLE – the client is video-capable but the video is not running (can occur during a manual send).
        /// STARTING – the video is sending but is not yet running at full speed.
        /// REJECTED – the receiver rejects the video feed (can occur during a manual receive).
        /// RUNNING – the video is actively running.
        /// STOPPING – the active video is in the process of stopping but has not halted yet.
        /// PAUSED – the video call is placed on hold.
        /// </summary>
        [SkypeParameter("VIDEO_RECEIVE_STATUS")]
        public string VideoReceiveStatus { get; set; }
        /// <summary>
        ///  – example: CALL 17 FAILUREREASON 1 (numeric).
        /// </summary>
        [SkypeParameter("FAILUREREASON")]
        public string FailureReason { get; set; }
        /// <summary>
        ///  – not used.
        /// </summary>
        [SkypeParameter("SUBJECT")]
        public string Subject { get; set; }
        /// <summary>
        ///  – example: CALL 17 PSTN_NUMBER 372123123 .
        /// </summary>
        [SkypeParameter("PSTN_NUMBER")]
        public string PSTNNumber { get; set; }
        /// <summary>
        ///  – example: CALL 17 DURATION 0 .
        /// </summary>
        [SkypeParameter("DURATION")]
        public string Duration { get; set; }
        /// <summary>
        ///  – error string from gateway, in the case of a PSTN call, for example: CALL 26 PSTN_STATUS 6500 PSTN connection creation timeout .
        /// </summary>
        [SkypeParameter("PSTN_STATUS")]
        public string PSTNStatus { get; set; }
        /// <summary>
        /// – number of non-hosts in the case of a conference call. Possible values are:
        /// 0 – call is not a conference. For the host, CONF_PARTICIPANTS_COUNT is always 0.
        /// 1 – call is a former conference.
        /// 2, 3, 4 – call is a conference. Note that from 2.5 and upwards, Skype API manages conference participation in a slightly different manner. In newer versions, after the call is finished, the CONF_PARTICIPANTS_COUNT reports highest number of participants the call had at any given time.
        /// </summary>
        [SkypeParameter("CONF_PARTICIPANTS_COUNT")]
        public string ConferenceCount { get; set; }
        /// <summary>
        /// n – the username of the nth participant in a conference call, 
        /// the call type and status and the displayname of participants 
        /// who are not the host. For example: CALL 59 CONF_PARTICIPANT 1 echo123 INCOMING_P2P INPROGRESS Echo Test Service .
        /// NOTE: !THIS NEEDS TO BE DONE SPECIALLY WHEN COUNT IS RECIEVED!
        /// </summary>
        [SkypeParameter("CONF_PARTICIPANT")]
        public string[] ConferenceParticipants { get; set; }
        /// <summary>
        /// Voicemail duration
        /// Applies only to calls which are forwarded into voicemail. This feature was introduced in protocol 5.
        /// </summary>
        [SkypeParameter("VM_DURATION")]
        public string VoicemailDuration { get; set; }
        /// <summary>
        ///  – maximum duration in seconds allowed to leave voicemail
        /// Applies only to calls which are forwarded into voicemail. This feature was introduced in protocol 5.
        /// </summary>
        [SkypeParameter("VM_ALLOWED_DURATION")]
        public string VoicemailDurationAllowed { get; set; }
        /// <summary>
        ///  – expressed as cost per minute (added in protocol 6).
        /// </summary>
        [SkypeParameter("RATE")]
        public string Rate { get; set; }
        /// <summary>
        ///  – EUR|USD.. This property gets populated from currency selected in Skype account details – PSTN_BALANCE_CURRENCY property of the PROFILE object. However, the value of PSTN_BALANCE_CURRENCY can change in time (added in protocol 6).
        /// </summary>
        [SkypeParameter("RATE_CURRENCY")]
        public string RateCurrency { get; set; }
        /// <summary>
        ///  – the number of times to divide RATE by 10 to get the full currency unit. For example, a RATE of 1234 with RATE_PRECISION of 2 amounts to 12.34 (added in protocol 6).
        /// </summary>
        [SkypeParameter("RATE_PRECISION")]
        public string RatePrecision { get; set; }
        /// <summary>
        ///  – New in API version 2.6 Refer to Voice Streams section for more information. Can have following values:
        /// SOUNDCARD="default" – default is currently the only acceptable value.
        /// PORT="port_no" – the ID of the audio port (1..65535)
        /// FILE="filename.wav" – the path and name of the audio file.
        /// </summary>
        [SkypeParameter("INPUT")]
        public string Input { get; set; }
        /// <summary>
        ///  Refer to Voice Streams section for more information. New in API version 2.6
        /// SOUNDCARD="default" – default is currently the only acceptable value.
        /// PORT="port_no" – the ID of the audio port (1..65535)
        /// FILE="filename.wav" – the path and name of the audio file.
        /// </summary>
        [SkypeParameter("OUTPUT")]
        public string Output { get; set; }
        /// <summary>
        /// Refer to Voice Streams section for more information. New in API version 2.6
        /// SOUNDCARD="default" – default is currently the only acceptable value.
        /// PORT="port_no" – the ID of the audio port (1..65535)
        /// FILE="filename.wav" – the path and name of the audio file.
        /// </summary>
        [SkypeParameter("CAPTURE_MIC")]
        public string CaptureMicrophone { get; set; }
        /// <summary>
        ///  – true|false, indicates if voice input is enabled. New in API version 2.6
        /// </summary>
        [SkypeParameter("VAA_INPUT_STATUS")]
        public string VAAInputStatus { get; set; }
        /// <summary>
        ///  – Contains identity of the user who forwarded a call. If the user who forwarded the call could not be identified then this property will be set to “?”. New in API version 2.6
        /// </summary>
        [SkypeParameter("FORWARDED_BY")]
        public string ForwardedBy { get; set; }

        public string[] Targets { get; set; }
    }
}
