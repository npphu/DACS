--Tao database
create database SFOOD
use SFOOD
go

--Tao Table
create table Admin
(
	MaTK_Admin int identity(1,1) primary key not null,
	TenTK_Admin varchar (30),
	MatKhau_Admin varchar (30),
)

create table VIP
(
	LevelVip int identity (1,1) primary key,
	SoTienCanTra decimal(12,0) check (SoTienCanTra >=0),
)

create table CHIECKHAU
(
	ID_ChiecKhau int identity (1,1) primary key not null,
	GiaTriCK float ,
)


create table NGUOIDUNG
(
	MaTK_User int identity (1,1) primary key not null,
	TenTK_User varchar (30),
	MatKhau_User varchar (30),
	HoTen nvarchar (50) not null,
	Email varchar (50),
	DiaChi nvarchar (200),
	DienThoai varchar (50),
	NgaySinh DateTime,
	GioiTinh bit,	
)

create table CONGTACVIEN
(
	MaTK_CTV int identity (1,1) primary key,
	TenTK_CTV varchar (30),
	MatKhau_CTV varchar (30),		
	HoTen nvarchar (50) not null,
	CMND varchar (10),
	Email varchar (50),
	DiaChi nvarchar (200),
	DienThoai varchar (50),
	NgaySinh DateTime,
	GioiTinh bit,	
	LevelVip int,
	TienVip decimal(12,0) check (TienVip >=0),
	NgayHetHanVip DateTime,
	foreign key (LevelVip) references VIP(LevelVip)
)

create table LOAIBUAAN
(
	ID_Loai int identity (1,1) primary key,
	TenLoai nvarchar (50),
)

create table KHUVUC
(
	ID_KhuVuc int identity (1,1) primary key,
	TenKhuVuc nvarchar (50),
)

create table MONAN
(
	ID_MonAn int identity (1,1) primary key not null,
	TenMonAn nvarchar (100),
	GiaBan decimal(12,0) check (GiaBan >=0),
	MoTa nvarchar(MAX),
	AnhBia varchar (100),
	NgayCapNhap DateTime,
	SoLuong int ,
	PhiVanChuyen decimal (12,0),
	ID_KhuVuc int,
	ID_Loai int,
	MaTK_CTV int,
	Constraint FK_KhuVuc foreign key (ID_KhuVuc) references KHUVUC(ID_KhuVuc),
	Constraint FK_Loai foreign key (ID_Loai) references LOAIBUAAN(ID_Loai),
	Constraint FK_CTV foreign key (MaTK_CTV) references CONGTACVIEN(MaTK_CTV)
)


create table DATHANG
(
	ID_DatHang int identity (1,1) primary key,
	MaTK_User int,
	ThoiGianDatHang DateTime ,
	DiaChiGiaoHang nvarchar (100),
	TinhTrangGiao bit,
	DaThanhToan bit,
	ThanhTien decimal(12,0) check (ThanhTien >=0)	
	Constraint FK_User foreign key (MaTK_User ) references NGUOIDUNG(MaTK_User ),

)


create table DONHANG
(
	ID_DatHang int ,
	ID_MonAn int ,
	SoLuong int check(SoLuong >=0),	
	Constraint PK_DonHang primary key (ID_DatHang,ID_MonAn) ,
	Constraint FK_DatHang foreign key (ID_DatHang) references DATHANG(ID_DatHang),
	Constraint FK_MonAn foreign key (ID_MonAn) references MONAN(ID_MonAn)
)

create table DOANHTHU
(

	MaTK_CTV int,
	TongDoanhThu decimal (19,0),
	TienChiecKhau decimal (19,0),
	Thang datetime,
	Constraint PK_DoanhThu primary key (MaTK_CTV),
	Constraint FK_CongTacVien foreign key (MaTK_CTV) references CONGTACVIEN(MaTK_CTV)

)

create table KhuyenMai
(	
	ID_MonAn int primary key not null,
	GiamGia float,
	foreign key (ID_MonAn) references MONAN(ID_MonAn) 
)
create table CAIDAT
(
	ID_CaiDat int identity (1,1) primary key ,
	TenThamSo varchar (30),
	HienThi nvarchar (50),
	GiaTri varchar (30)
)

--Ham tinh tong doanh thu
create function f_tongdt (@idctv int)
returns decimal(12, 0)
as
begin
	declare @tong decimal(12, 0)
	set @tong = 0

	select @tong = @tong + sum(SoLuong * GiaBan + PhiVanChuyen) 
	from
	(
		select m.ID_MonAn, d.SoLuong, PhiVanChuyen, GiaBan, GiamGia
		from MONAN m 
		left join KhuyenMai k on m.ID_MonAn = k.ID_MonAn 
		inner join DONHANG d on m.ID_MonAn = d.ID_MonAn
		where m.MaTK_CTV = @idctv
	) as T1
	where GiamGia is null

	declare @tongkm decimal(12, 0)
	set @tongkm = 0

	select @tongkm = sum(SoLuong * GiaBan * (1-(GiamGia)/100) + PhiVanChuyen) 
	from
	(
		select m.ID_MonAn, d.SoLuong, PhiVanChuyen, GiaBan, GiamGia
		from MONAN m 
		left join KhuyenMai k on m.ID_MonAn = k.ID_MonAn 
		inner join DONHANG d on m.ID_MonAn = d.ID_MonAn
		where m.MaTK_CTV = @idctv
	) as T1
	where GiamGia is not null

	if (@tong is null)
	begin
		set @tong = 0
	end

	if (@tongkm is not null)
	begin
		set @tong = @tong + @tongkm
	end

	return @tong
end

--trigger----------------------------------------------
create trigger T_TongDoanhThu
on DonHang
after insert, update, delete
as
begin
	declare @idctv int
	if ((select count(*) from inserted) > 0)
	begin
		select @idctv = m.MaTK_CTV from inserted i, MONAN m where i.ID_MonAn = m.ID_MonAn
		
	end
	else
	begin
		select @idctv = m.MaTK_CTV from deleted i, MONAN m where i.ID_MonAn = m.ID_MonAn

	end

	update DOANHTHU
	set TongDoanhThu = dbo.f_tongdt(@idctv)
	where MaTK_CTV = @idctv
end

--Procedure Cai dat
create proc p_UpdateCaiDat @TenThamSo varchar(50), @GiaTri nvarchar(100)
as
	update CaiDat
	set GiaTri = @GiaTri
	where TenThamSo = @TenThamSo

--top 5 mon an ban chay
create view V_Top5MonAn
as
	select top 5 with ties TenMonAn, M.MaTK_CTV , sum(D.SoLuong) as [Số lượng] 
	from DonHang D, MONAN M 
	where D.ID_MonAn = M.ID_MonAn
	group by TenMonAn, M.MaTK_CTV
	order by SUM(D.SoLuong) desc

--top 5 khach hang mua nhieu nhat
create view V_Top5KhachHang
as
	select top 5 with ties HoTen ,DienThoai , M.MaTK_CTV, sum(D.ThanhTien) as [Tổng tiền] 
	from DATHANG D, NGUOIDUNG N, MONAN M, DONHANG DH
	where D.MaTK_User = N.MaTK_User and DH.ID_MonAn = M.ID_MonAn and DH.ID_DatHang = d.ID_DatHang
	group by HoTen, DienThoai , M.MaTK_CTV
	order by sum(D.ThanhTien) desc
