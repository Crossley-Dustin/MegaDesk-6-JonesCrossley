using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaDesk_6_JonesCrossley
{
    public class Desk
    {
        public int Width { get; set; }
        public int Depth { get; set; }
        public int DrawerCount { get; set; }
        public DesktopMaterial Surface { get; set; }

        public enum DesktopMaterial
        {
            Laminate = 100, Oak = 200, Rosewood = 300, Veneer = 125, Pine = 50
        };

        public int GetSurfaceArea()
        {
            int area;
            area = Width * Depth;
            return area;
        }
    }
}
