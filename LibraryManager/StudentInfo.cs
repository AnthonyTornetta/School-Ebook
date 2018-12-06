namespace LibraryManager
{
    public class StudentInfo
    {
        public string Name { get; set; }
        public EBook EBook { get; set; }
        public int Grade { get; set; }

        public StudentInfo(string name, int grade, EBook ebook)
        {
            Name = name;
            Grade = grade;
            EBook = ebook;
        }

        public StudentInfo() : this("", -1, null)
        {}

        public override bool Equals(object obj)
        {
            if (obj is StudentInfo other)
                if (other.Name.Equals(Name))
                    if (other.EBook == null && EBook == null || (other.EBook != null && EBook != null && other.EBook.Equals(EBook)))
                        if (other.Grade == Grade)
                            return true;
            return false;
        }

        public override int GetHashCode()
        {
            int hashcode = 1;
            const int prime = 31;

            hashcode = hashcode * prime + Name.GetHashCode();
            hashcode = hashcode * prime + Grade;
            hashcode = hashcode * prime + EBook.GetHashCode();

            return hashcode;
        }
    }
}
