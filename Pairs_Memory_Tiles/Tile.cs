namespace Pairs_Bajan_Ramona
{
    public class Tile
    {
        public string image;
        public string back = "i0.jpg";
        public string imageInBinding = "i0.jpg";
        public bool flip = false;
        public bool done = false;

        public Tile()
        {

        }

        public Tile(Tile t)
        {
            image = t.image;
            flip = t.flip;
            done = t.done;
            imageInBinding = t.imageInBinding;
        }

        public Tile(string image, bool flip, bool done, string imageInBinding) // modified constructor
        {
            this.image = image;
            this.flip = flip;
            this.done = done;
            this.imageInBinding = imageInBinding;
        }

        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        public string ImageInBinding
        {
            get { return imageInBinding; }
            set { imageInBinding = value; }
        }

        public bool Flip
        {
            get { return flip; }
            set { flip = value; }
        }

        public bool Done
        {
            get { return done; }
            set { done = value; }
        }

    }
}
