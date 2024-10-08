namespace CGraphic {
    public enum ImageType {
        PNG,
        JPG,
        GIF,
        BMP,
        None
    }

    public class ImageUtils {
        public static ImageType GetImageType(byte[] data) {
            if (data.Length < 2) return ImageType.None;
            //根据文件头判断
            string strFlag = data[0].ToString() + data[1].ToString();
            //察看格式类型
            switch (strFlag) {
                //JPG格式
                case "255216":
                    return ImageType.JPG;
                //GIF格式
                case "7173":
                    return ImageType.GIF;
                //BMP格式
                case "6677":
                    return ImageType.BMP;
                //PNG格式
                case "13780":
                    return ImageType.PNG;
            }

            return ImageType.None;
        }
    }
}