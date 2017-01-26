#region Assembly S22.Mail, Version=1.0.4668.31819, Culture=neutral, PublicKeyToken=null
// D:\Work\Escc.Services.Azure\packages\S22.Mail.1.0.4668.31819\lib\net45\S22.Mail.dll
#endregion

using S22.Mail;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System;

namespace Escc.Services.Azure
{
    //
    // Summary:
    //     A serializable replication of the MailMessage class of the System.Net.Mail namespace.
    //     It implements conversion operators to allow for implicit conversion between SerializableMailMessage
    //     and MailMessage objects.

    // Update: The only changes here are to 'NameValueCollection Headers' and a minor change to the implicit operators.
    // Json has some issues serializing a NameValueCollection so some extra work is needed to handle the conversion using a dictionary.
    // Everything else is the same as the SerialiazableMailMessage class from S22.Mail.
    [Serializable]
    public class SerializableJsonMailMessage
    {
        public SerializableAlternateViewCollection AlternateViews { get; }
        public SerializableAttachmentCollection Attachments { get; }
        public SerializableMailAddressCollection Bcc { get; }
        public string Body { get; set; }
        public Encoding BodyEncoding { get; set; }
        public SerializableMailAddressCollection CC { get; }
        public DeliveryNotificationOptions DeliveryNotificationOptions { get; set; }
        public SerializableMailAddress From { get; set; }
        public NameValueCollection Headers
        {
            get
            {
                var nvc = new NameValueCollection();
                CollDictionary.Keys.ToList().ForEach(a => nvc.Add(a, CollDictionary[a]));
                return nvc;
            }
            set
            {
                CollDictionary = value.AllKeys.ToDictionary(k => k, k => value[k]);
            }
        }
        public Dictionary<string, string> CollDictionary { get; set; }
        public Encoding HeadersEncoding { get; set; }
        public bool IsBodyHtml { get; set; }
        public MailPriority Priority { get; set; }
        public SerializableMailAddress ReplyTo { get; set; }
        public SerializableMailAddressCollection ReplyToList { get; }
        public SerializableMailAddress Sender { get; set; }
        public string Subject { get; set; }
        public Encoding SubjectEncoding { get; set; }
        public SerializableMailAddressCollection To { get; }

        public static implicit operator MailMessage(SerializableJsonMailMessage message)
        {
            MailMessage m = new MailMessage();
            foreach (SerializableAlternateView a in message.AlternateViews)
                m.AlternateViews.Add(a);
            foreach (SerializableAttachment a in message.Attachments)
                m.Attachments.Add(a);
            foreach (SerializableMailAddress a in message.Bcc)
                m.Bcc.Add(a);
            m.Body = message.Body;
            m.BodyEncoding = message.BodyEncoding;
            foreach (SerializableMailAddress a in message.CC)
                m.CC.Add(a);
            m.DeliveryNotificationOptions = message.DeliveryNotificationOptions;
            m.From = message.From;
            m.Headers.Add(message.Headers);
            m.HeadersEncoding = message.HeadersEncoding;
            m.IsBodyHtml = message.IsBodyHtml;
            m.Priority = message.Priority;
            m.ReplyTo = message.ReplyTo;
            foreach (SerializableMailAddress a in message.ReplyToList)
                m.ReplyToList.Add(a);
            m.Sender = message.Sender;
            m.Subject = message.Subject;
            m.SubjectEncoding = message.SubjectEncoding;
            foreach (SerializableMailAddress a in message.To)
                m.To.Add(a);
            return m;
        }
        public static implicit operator SerializableJsonMailMessage(MailMessage message)
        {
            return new SerializableJsonMailMessage(message);
        }

        private SerializableJsonMailMessage(MailMessage m)
        {
            AlternateViews = new SerializableAlternateViewCollection();
            foreach (AlternateView a in m.AlternateViews)
                AlternateViews.Add(a);
            Attachments = new SerializableAttachmentCollection();
            foreach (Attachment a in m.Attachments)
                Attachments.Add(a);
            Bcc = new SerializableMailAddressCollection();
            foreach (MailAddress a in m.Bcc)
                Bcc.Add(a);
            Body = m.Body;
            BodyEncoding = m.BodyEncoding;
            CC = new SerializableMailAddressCollection();
            foreach (MailAddress a in m.CC)
                CC.Add(a);
            DeliveryNotificationOptions = m.DeliveryNotificationOptions;
            From = m.From;
            Headers = new NameValueCollection();
            Headers.Add(m.Headers);
            HeadersEncoding = m.HeadersEncoding;
            IsBodyHtml = m.IsBodyHtml;
            Priority = m.Priority;
            ReplyTo = m.ReplyTo;
            ReplyToList = new SerializableMailAddressCollection();
            foreach (MailAddress a in m.ReplyToList)
                ReplyToList.Add(a);
            Sender = m.Sender;
            Subject = m.Subject;
            SubjectEncoding = m.SubjectEncoding;
            To = new SerializableMailAddressCollection();
            foreach (MailAddress a in m.To)
                To.Add(a);
        }
    }
}