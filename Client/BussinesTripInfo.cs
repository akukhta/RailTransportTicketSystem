using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class BussinesTripInfo
    {
        public string name, surname, patronymic, job, destinationPlace, reason, fullNameOfSender;
        public DateTime from, to;

        public BussinesTripInfo(string name, string surname, string patronymic, string job, string destinationPlace, string reason,
            string fullNameOfSender, DateTime from, DateTime to)
        {
            this.name = name;
            this.surname = surname;
            this.patronymic = patronymic;
            this.job = job;
            this.destinationPlace = destinationPlace;
            this.reason = reason;
            this.fullNameOfSender = fullNameOfSender;
            this.from = from;
            this.to = to;
        }

        public byte[] serialise()
        {
            List<byte> buffer = new List<byte>();

            buffer.AddRange(BitConverter.GetBytes(name.Length));
            buffer.AddRange(Encoding.Default.GetBytes(name));
            buffer.AddRange(BitConverter.GetBytes(surname.Length));
            buffer.AddRange(Encoding.Default.GetBytes(surname));
            buffer.AddRange(BitConverter.GetBytes(patronymic.Length));
            buffer.AddRange(Encoding.Default.GetBytes(patronymic));
            buffer.AddRange(BitConverter.GetBytes(job.Length));
            buffer.AddRange(Encoding.Default.GetBytes(job));
            buffer.AddRange(BitConverter.GetBytes(destinationPlace.Length));
            buffer.AddRange(Encoding.Default.GetBytes(destinationPlace));
            buffer.AddRange(BitConverter.GetBytes(reason.Length));
            buffer.AddRange(Encoding.Default.GetBytes(reason));
            buffer.AddRange(BitConverter.GetBytes(fullNameOfSender.Length));
            buffer.AddRange(Encoding.Default.GetBytes(fullNameOfSender));
            buffer.AddRange(BitConverter.GetBytes(from.Ticks));
            buffer.AddRange(BitConverter.GetBytes(to.Ticks));

            return buffer.ToArray();
        }

        public BussinesTripInfo deserialise(List<byte> buffer)
        {
            string name = Utilites.readStringFromBuffer(buffer);
            string surname = Utilites.readStringFromBuffer(buffer);
            string patronymic = Utilites.readStringFromBuffer(buffer);
            string job = Utilites.readStringFromBuffer(buffer);
            string destinationPlace = Utilites.readStringFromBuffer(buffer);
            string reason = Utilites.readStringFromBuffer(buffer);
            string fullNameOfSender = Utilites.readStringFromBuffer(buffer);
            DateTime from = DateTime.FromBinary(BitConverter.ToInt64(buffer.ToArray(), 0));
            buffer.RemoveRange(0, sizeof(Int64));
            DateTime to = DateTime.FromBinary(BitConverter.ToInt64(buffer.ToArray(), 0));

            return new BussinesTripInfo(name, surname, patronymic, job, destinationPlace, reason, fullNameOfSender, from, to);
        }
    }
}
