namespace BlazorChifoumi
{
	public static class HandExtensions
	{
		public static string ToImage(this Hand source)
		{
			switch (source) {
				case Hand.pierre:
					return "pierre.png";
				case Hand.feuille:
					return "feuille.png";
				case Hand.ciseaux:
					return "ciseaux.png";
				default:
					return "think.jpg";
			}
		}
	}
}