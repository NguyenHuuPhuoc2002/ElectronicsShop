using System;
using System.Collections.Generic;

namespace EcommerceWeb.Data;

public partial class HoiDap
{
    public int MaHd { get; set; }

    public string CauHoi { get; set; }

    public string TraLoi { get; set; }

    public DateOnly NgayDua { get; set; }

    public string MaNv { get; set; }

    public virtual NhanVien MaNvNavigation { get; set; }
}
