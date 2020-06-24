use ConsentManagementDB
go
--#1
if not exists (select * from sysobjects where name='UserLogin' and xtype='u')
    create table UserLogin (
		id UNIQUEIDENTIFIER DEFAULT NEWID() primary key,
        user_no int not null   identity(1,1),
		user_name varchar(50),
		user_create datetime DEFAULT getdate(),
		user_last_login datetime DEFAULT getdate(),
		user_status varchar(10) DEFAULT  'Y'
    )
	--insert into UserLogin (user_name) values('backofficeservice@asiaplus.co.th')
go

if not exists (select * from sysobjects where name='FunctionControl' and xtype='u')
    create table FunctionControl (
		function_name varchar(150),
        is_use char(1)
    )
	--insert into FunctionControl (function_name,is_use) values('UsersManagement','Y')
go
if not exists (select * from sysobjects where name='UserPermissionControl' and xtype='u')
    create table UserPermissionControl (
		function_name varchar(150),
		user_name varchar(50)
    )
	--insert into UserPermissionControl (function_name,user_name) values('UsersManagement','backofficeservice@asiaplus.co.th')
	--insert into UserPermissionControl (function_name,user_name) values('DashBoard','backofficeservice@asiaplus.co.th')
	--insert into UserPermissionControl (function_name,user_name) values('CustomerAcceptedConsents','backofficeservice@asiaplus.co.th')
go

if not exists (select * from sysobjects where name='UserActivityLog' and xtype='u')
    create table UserActivityLog (
		user_name varchar(50),
		activity varchar(max) ,
		remark varchar(max) ,
		log_date_time datetime DEFAULT getdate()
    )
	--drop table UserActivityLog
	--insert into UserLogin (user_name) values('backofficeservice@asiaplus.co.th')
go

--#2
if not exists (select * from sysobjects where name='CustomerAcceptedConsentForm' and xtype='u')
    create table CustomerAcceptedConsentForm (
		id UNIQUEIDENTIFIER DEFAULT NEWID() primary key,
		account_no varchar(10) not null ,
		ref_no varchar(20)  null ,
		product_name varchar(30)  null ,
		consent_form_id  UNIQUEIDENTIFIER not null,
		consent_date datetime not null,
		is_use varchar(1) not null,
		system_name varchar(150),
		system_ip_address varchar(50),
		customer_ip_address varchar(50),
		mac_address varchar(50),
		remark varchar(255),
		update_flag char(1)
    )
	--ALTER TABLE CustomerAcceptedConsentForm
	--ADD ref_no varchar(20)  null
	--ALTER TABLE CustomerAcceptedConsentForm
	--ADD product_name varchar(30)  null
	--insert into CustomerAcceptedConsentForm(account_no,consent_form_id,consent_date,is_use)
	--values('X1111','B27AB9EA-56DC-4369-B3F9-F3D1CCFC49E6',GETDATE(),'Y')
	
go
if not exists (select * from sysobjects where name='CustomerAcceptedConsentForm_Log' and xtype='u')
    create table CustomerAcceptedConsentForm_Log (
		id UNIQUEIDENTIFIER not null,
		account_no varchar(10) not null ,
		ref_no varchar(20) not null ,
		product_name varchar(30) not null ,
		consent_form_id  UNIQUEIDENTIFIER not null,
		consent_date datetime ,
		is_use varchar(1),
		system_name varchar(150),
		system_ip_address varchar(50),
		customer_ip_address varchar(50),
		mac_address varchar(50),
		remark varchar(255),
		update_flag char(1)
    )
	--ALTER TABLE CustomerAcceptedConsentForm_Log
	--ADD ref_no varchar(20)  null
	--ALTER TABLE CustomerAcceptedConsentForm_Log
	--ADD product_name varchar(30)  null
go
--#3
if not exists (select * from sysobjects where name='CustomerAcceptedConsentDetail' and xtype='u')
    create table CustomerAcceptedConsentDetail (
		id UNIQUEIDENTIFIER DEFAULT NEWID() primary key,
		account_no varchar(10) not null,
		consent_id  UNIQUEIDENTIFIER not null,
		is_accepted char(1) not null,
		create_datetime datetime not null,
		update_flag char(1)
    )
	--insert into CustomerAcceptedConsentDetail (account_no,consent_id,is_accepted,create_datetime)
	--values('X1111','6F24745C-40EC-4AC7-A933-9D6B4F589FFF','Y',GETDATE())
	--		,('X1111','F73D49FD-EB8D-44EC-AC6D-D702F8327133','Y',GETDATE())
	--		,('X1111','342EFAC3-E78E-4F91-B2AF-E7B7DA364D82','Y',GETDATE())
go
if not exists (select * from sysobjects where name='CustomerAcceptedConsentDetail_Log' and xtype='u')
    create table CustomerAcceptedConsentDetail_Log (
		id UNIQUEIDENTIFIER not null,
		account_no varchar(10) not null,
		consent_id  UNIQUEIDENTIFIER not null,
		is_accepted char(1) not null,
		create_datetime datetime not null,
		update_flag char(1)
    )
go
------------------------------------------------------------------------------
------------------------------------------------------------------------------
------------------------------------------------------------------------------
--#4
if not exists (select * from sysobjects where name='ConsentForm' and xtype='u')
    create table ConsentForm (
		consent_form_id UNIQUEIDENTIFIER DEFAULT NEWID() primary key,
		consent_form_name  varchar(50),
		consent_title varchar(255),
		consent_form_header varchar(max),
		consent_form_footer varchar(max),
		consent_form_version decimal(6,2) ,
		create_date datetime default getdate() ,
		use_status char(1) default 'Y',
		remark varchar(255)
    )
 --insert into ConsentForm(consent_form_name,consent_form_version,consent_title,consent_form_header,consent_form_footer)
 --select 'ASCO_CONSENT_THAI_1',1.00,'หนังสือยินยอมของลูกค้า','โปรดแจ้งให้เราทราบถึงความพึงพอใจและความตกลงของคุณตามที่ปรากฎด้านล่างนี้ คำที่นิยามไว้ในนโยบายความเป็นส่วนตัวของ [ชื่อบริษัท] จะมีความหมายเช่นเดียวกันเมื่อใช้ในเอกสารฉบับนี้ เว้นแต่นิยามไว้เป็นอย่างอื่นในเอกสารฉบับนี้',''

--insert into ConsentForm(consent_form_name,consent_form_version,consent_title,consent_form_header,consent_form_footer)
--select 'ASCO_CONSENT_ENG_1',1.00,'Customer Consent Form','Please let us know your preference and agreement below. Terms defined in [Company name]’s Privacy Policy will, unless otherwise defined herein, have the same meaning when used in this document. ',''
go
--#5
if not exists (select * from sysobjects where name='ConsentDetail' and xtype='u')
    create table consentdetail (
		consent_id uniqueidentifier default newid() primary key,
		consent_form_id uniqueidentifier,
		title_name  varchar(150),
		consent_order int,
		content_html varchar(max),
		content_text varchar(max),
		remark varchar(max),
		create_date datetime default getdate() 
    )
	--insert into ConsentDetail(consent_form_id,title_name,consent_order,content)
	--select 'B27AB9EA-56DC-4369-B3F9-F3D1CCFC49E6','title1',1,'<p style=''Color:Red;Font-Size:2px''>xyz xyz xyz</p><image src="~xyz/xyz.png" alt="image"/>'
	--insert into ConsentDetail(consent_form_id,title_name,consent_order,content)
	--select 'B27AB9EA-56DC-4369-B3F9-F3D1CCFC49E6','title2',1,'<p style=''Color:Red;Font-Size:2px''>xyz xyz zcxvxzvzxcvz zsdxzcvzxvcxzvzx vzxvcxzcvzxv xyz</p><image src="~xyz/xyz.png" alt="image"/>'
	--insert into ConsentDetail(consent_form_id,title_name,consent_order,content)
	--select 'B27AB9EA-56DC-4369-B3F9-F3D1CCFC49E6','title2',1,'<p style=''Color:Red;Font-Size:2px''>xyz xyz zcxvxzvzxcvz zsdxzcvzxvcxzvzx vzxvcxzcvzxv xyz</p><image src="~xyz/xyz.png" alt="image"/>'
	--insert into ConsentDetail(consent_form_id,title_name,consent_order,content_html,content_text)
	--select '6C074227-BE2C-4592-8775-26932ACA5DDE','',1
	--,'ข้าพเจ้าได้อ่านและตกลงตาม<U>ข้อกำหนดและเงื่อนไข</U> [*ใส่ลิงค์ที่นี่] และรับทราบ<U>นโยบายความเป็นส่วนตัว</U> [*ใส่ลิงค์ที่นี่] ซึ่งระบุถึงวิธีการที่ [ชื่อบริษัท] ("บริษัท" "เรา" หรือ "ของเรา") จะเก็บรวบรวม ใช้ เปิดเผย และ/หรือโอนข้อมูลส่วนบุคคลและข้อมูลที่ละเอียดอ่อนของข้าพเจ้าไปยังต่างประเทศแล้ว'
	--,'ข้าพเจ้าได้อ่านและตกลงตามข้อกำหนดและเงื่อนไข [*ใส่ลิงค์ที่นี่] และรับทราบนโยบายความเป็นส่วนตัว [*ใส่ลิงค์ที่นี่] ซึ่งระบุถึงวิธีการที่ [ชื่อบริษัท] ("บริษัท" "เรา" หรือ "ของเรา") จะเก็บรวบรวม ใช้ เปิดเผย และ/หรือโอนข้อมูลส่วนบุคคลและข้อมูลที่ละเอียดอ่อนของข้าพเจ้าไปยังต่างประเทศแล้ว'
	--insert into ConsentDetail(consent_form_id,title_name,consent_order,content_html,content_text)
	--select '6C074227-BE2C-4592-8775-26932ACA5DDE','',2
	--,'ข้าพเจ้ายินยอมให้มีการเก็บรวบรวม ใช้ และ/หรือเปิดเผยข้อมูลส่วนบุคคลของข้าพเจ้า เพื่อวัตถุประสงค์ในการรับการติดต่อสื่อสารทางการตลาด ข้อเสนอพิเศษ เอกสารส่งเสริมการขายที่เกี่ยวกับผลิตภัณฑ์และบริการของบริษัท บริษัทในเครือและบริษัทย่อยของบริษัท และบุคคลภายนอกซึ่งเราไม่สามารถอาศัยหลักเกณฑ์หรือฐานทางกฎหมายอื่น'
	--,'ข้าพเจ้ายินยอมให้มีการเก็บรวบรวม ใช้ และ/หรือเปิดเผยข้อมูลส่วนบุคคลของข้าพเจ้า เพื่อวัตถุประสงค์ในการรับการติดต่อสื่อสารทางการตลาด ข้อเสนอพิเศษ เอกสารส่งเสริมการขายที่เกี่ยวกับผลิตภัณฑ์และบริการของบริษัท บริษัทในเครือและบริษัทย่อยของบริษัท และบุคคลภายนอกซึ่งเราไม่สามารถอาศัยหลักเกณฑ์หรือฐานทางกฎหมายอื่น'
	--insert into ConsentDetail(consent_form_id,title_name,consent_order,content_html,content_text)
	--select '6C074227-BE2C-4592-8775-26932ACA5DDE','',3
	--,'ข้าพเจ้ายินยอมให้มีการเก็บรวบรวม ใช้ และ/หรือเปิดเผยข้อมูลที่ละเอียดอ่อนของข้าพเจ้า เพื่อวัตถุประสงค์ที่ระบุในนโยบายความเป็นส่วนตัว [*ใส่ลิงค์ที่นี่]'
	--,'ข้าพเจ้ายินยอมให้มีการเก็บรวบรวม ใช้ และ/หรือเปิดเผยข้อมูลที่ละเอียดอ่อนของข้าพเจ้า เพื่อวัตถุประสงค์ที่ระบุในนโยบายความเป็นส่วนตัว [*ใส่ลิงค์ที่นี่]'
	--insert into ConsentDetail(consent_form_id,title_name,consent_order,content_html,content_text)
	--select '6C074227-BE2C-4592-8775-26932ACA5DDE','',4
	--,'[ข้าพเจ้ายินยอมให้มีการโอนข้อมูลส่วนบุคคลของข้าพเจ้าไปยังประเทศซึ่งอาจไม่มีระดับการคุ้มครองข้อมูลที่เพียงพอ ซึ่งกฎหมายกำหนดให้ต้องได้รับความยินยอม]'
	--,'[ข้าพเจ้ายินยอมให้มีการโอนข้อมูลส่วนบุคคลของข้าพเจ้าไปยังประเทศซึ่งอาจไม่มีระดับการคุ้มครองข้อมูลที่เพียงพอ ซึ่งกฎหมายกำหนดให้ต้องได้รับความยินยอม]'
	--insert into ConsentDetail(consent_form_id,title_name,consent_order,content_html,content_text)
	--select 'D51E28B3-F691-4BE3-8B2D-6E78B11405B9','',1
	--,'I have read and agreed to the Terms and Conditions [*insert link here] and acknowledged the Privacy Policy [*insert link here] which describes how [Company name] (the "Company," "we," "us," or "our") will collect, use, disclose, and/or cross-border transfer my Personal Data and Sensitive Data'
	--,'I have read and agreed to the Terms and Conditions [*insert link here] and acknowledged the Privacy Policy [*insert link here] which describes how [Company name] (the "Company," "we," "us," or "our") will collect, use, disclose, and/or cross-border transfer my Personal Data and Sensitive Data'
	--insert into ConsentDetail(consent_form_id,title_name,consent_order,content_html,content_text)
	--select 'D51E28B3-F691-4BE3-8B2D-6E78B11405B9','',2
	--,'I consent to the collection, use and/or disclosure of my Personal Data for the purpose of receiving marketing communications, special offers, promotional materials about the products and services of the Company, our affiliates and subsidiaries and the third parties which we cannot rely on other legal grounds.'
	--,'I consent to the collection, use and/or disclosure of my Personal Data for the purpose of receiving marketing communications, special offers, promotional materials about the products and services of the Company, our affiliates and subsidiaries and the third parties which we cannot rely on other legal grounds.'
	--insert into ConsentDetail(consent_form_id,title_name,consent_order,content_html,content_text)
	--select 'D51E28B3-F691-4BE3-8B2D-6E78B11405B9','',3
	--,'I consent to the collection, use and/or disclosure of my Sensitive Data for the purposed prescribed in the Privacy Policy [*insert link here].'
	--,'I consent to the collection, use and/or disclosure of my Sensitive Data for the purposed prescribed in the Privacy Policy [*insert link here].'
	--insert into ConsentDetail(consent_form_id,title_name,consent_order,content_html,content_text)
	--select 'D51E28B3-F691-4BE3-8B2D-6E78B11405B9','',4
	--,'[I consent to the cross-border transfer of my Personal Data to a country which may not have an adequate level of data protection, for which consent is required by law].'
	--,'[I consent to the cross-border transfer of my Personal Data to a country which may not have an adequate level of data protection, for which consent is required by law].'

go
--#6
if not exists (select * from sysobjects where name='ConsentSystemAllow' and xtype='u')
    create table ConsentSystemAllow (
		consent_form_id UNIQUEIDENTIFIER ,
		system_name  varchar(50),
		system_ip_address varchar(20),
		use_status char(1) default 'Y',
		remark varchar(255)
    )
-- insert into ConsentSystemAllow(consent_form_id,system_name)select 'B27AB9EA-56DC-4369-B3F9-F3D1CCFC49E6','SystemTest'
go


CREATE TABLE [dbo].[CustomerData](
	[AccountNo] [varchar](5) NOT NULL,
	[ReferenceID] [varchar](13) NOT NULL,
	[NameEn] [varchar](110) NOT NULL,
	[NameTh] [varchar](110) NOT NULL
) ON [PRIMARY]
GO

------------------------------------------------------------------------------
------------------------------------------------------------------------------
------------------------------------------------------------------------------


