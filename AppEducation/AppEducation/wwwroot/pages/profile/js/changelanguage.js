

/*<img id="ChangeLanguage" src="~/pages/profile/images/flag_vi.png" alt="language_flag" class="flag_uk mr-2">
    Tiếng Việt
                                        <span class="caret"></span>*/
$(document).ready(function () {
  
    $("#TiengAnh").click(function() {
        var TiengViet = document.getElementById('TiengViet')
        var ThongBao = document.getElementById('ThongBao')
        var HoTro = document.getElementById('HoTro')
        var HocTrucTuyen = document.getElementById('HocTrucTuyen')
        var TinTuc = document.getElementById('TinTuc')
        var KiemTra = document.getElementById('KiemTra')
        var aChangelanguage = document.getElementById('aChangeLanguage')
        var DanhSachLop = document.getElementById('DanhSachLop')
        var DangKyLop = document.getElementById('DangKyLop')
        DangKyLop.innerHTML = 'Create new class'
        DanhSachLop.innerHTML = 'Classes'
        KiemTra.innerHTML = 'Examine'
        $(this).innerHTML = 'English'
        TiengViet.innerHTML = 'Vietnamese'
        ThongBao.innerHTML = 'Notification'
        HoTro.innerHTML = 'Support'
        HocTrucTuyen.innerHTML = 'Learning Online'
        TinTuc.innerHTML = 'News'
        aChangelanguage.innerHTML = '<img src="/pages/profile/images/flag_uk.png" alt="language_flag" class="flag_uk mr-2">English'
        })
    $("#TiengViet").click(function () {
        var aChangelanguage = document.getElementById('aChangeLanguage')
        var KiemTra = document.getElementById('KiemTra')
        var TiengAnh = document.getElementById('TiengAnh')
        var ThongBao = document.getElementById('ThongBao')
        var HoTro = document.getElementById('HoTro')
        var HocTrucTuyen = document.getElementById('HocTrucTuyen')
        var TinTuc = document.getElementById('TinTuc')
        var DangKyLop = document.getElementById('DangKyLop')
        DangKyLop.innerHTML = 'Đăng ký lớp'
        TiengAnh.innerHTML = 'Tiếng Anh'
        $(this).innerHTML = 'Tiếng Việt'
        ThongBao.innerHTML = 'Thông báo'
        HoTro.innerHTML = 'Hỗ Trợ'
        HocTrucTuyen.innerHTML = 'Học Trực Tuyến'
        TinTuc.innerHTML = 'Tin tức'
        KiemTra.innerHTML = 'Kiểm Tra'
        var DanhSachLop = document.getElementById('DanhSachLop')
        DanhSachLop.innerHTML = 'Danh Sách Lớp'
        aChangelanguage.innerHTML = '<img src="/pages/profile/images/flag_vi.png" alt="language_flag" class="flag_uk mr-2">Tiếng Việt'

    })
})