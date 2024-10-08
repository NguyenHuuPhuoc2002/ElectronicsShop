﻿using System;
using System.Collections.Generic;

namespace EcommerceWeb.Data;

public partial class NhanVien
{
    public string MaNv { get; set; }

    public string HoTen { get; set; }

    public string Email { get; set; }

    public string MatKhau { get; set; }

    public string MaPb { get; set; }

    public virtual ICollection<ChuDe> ChuDes { get; set; } = new List<ChuDe>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<HoiDap> HoiDaps { get; set; } = new List<HoiDap>();

    public virtual ICollection<PhanCong> PhanCongs { get; set; } = new List<PhanCong>();
}
