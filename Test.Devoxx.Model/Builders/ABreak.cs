using Devoxx.Model;

namespace Test.Devoxx.Model.Builders
{
    public class ABreak
    {
        private string id;
        private string nameEn;
        private string nameFr;
        private Room room;


        public ABreak TakingPlaceIn(Room room)
        {
            this.room = room;
            return this;
        }

        public ABreak WithNameFr(string nameFr)
        {
            this.nameFr = nameFr;
            return this;
        }

        public ABreak WithNameEn(string nameEn)
        {
            this.nameEn = nameEn;
            return this;
        }

        public Break Build()
        {
            return new Break(id, room, nameEn, nameFr);
        }

    }
}
