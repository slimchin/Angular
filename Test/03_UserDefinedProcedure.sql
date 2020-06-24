use [consentmanagementDB]
go
if exists (
        select type_desc, type
        from sys.procedures with(nolock)
        where name = 'udp_cnms_save_activity_log'
            and type = 'p'
      )
     drop procedure dbo.udp_cnms_save_activity_log
go
create proc dbo.udp_cnms_save_activity_log
		--@pid varchar,
        --@puser_id int,
		@puser_name varchar(255),
		@pactivity varchar(max) ,
		@premark varchar(max) = null
as
begin 

	begin try  
		begin transaction

		insert into useractivitylog(user_name,activity,remark,log_date_time)
		values(@puser_name,@pactivity,@premark,getdate())

		update [dbo].[UserLogin]
		set user_last_login = GETDATE()
		where user_name = @puser_name

		commit
		select 0 errornumber,'' errormessage

	end try  
	begin catch  
		rollback
		select   
			error_number() as errornumber  
			,error_message() as errormessage;  
	end catch;  
end
go

-- add new customer_accepted_consent_form
if exists (
        select type_desc, type
        from sys.procedures with(nolock)
        where name = 'udp_cnms_save_customer_accepted_consent_form'
            and type = 'p'
      )
     drop procedure dbo.udp_cnms_save_customer_accepted_consent_form
go

--  exec udp_cnms_save_customer_accepted_consent_form '99977','b27ab9ea-56dc-4369-b3f9-f3d1ccfc49e6','','','','','',''
create proc dbo.udp_cnms_save_customer_accepted_consent_form
		@paccount_no varchar(10) ,
		@pconsent_form_id  uniqueidentifier,
		@psystem_name varchar(100) ,
		@psystem_ip_address varchar(50) ,
		@pcustomer_ip_address varchar(50) ,
		@pproduct_name varchar(30),
		@pmac_address varchar(50),
		@premark varchar(255)
as
begin 

	begin try  
		begin transaction
			if(len(@paccount_no)>5)
				set @paccount_no =upper( SUBSTRING(@paccount_no,len(@paccount_no)-4,len(@paccount_no)))
			declare @vref_no varchar(20)
			select @vref_no= ReferenceID from  [dbo].[CustomerData] where AccountNo = @paccount_no
			

			declare @vtransaction_id uniqueidentifier = newid()
			if exists (select consent_form_id from dbo.CustomerAcceptedConsentForm where ref_no = @vref_no)
				-- raiserror with severity 11-19 will cause execution to   
				-- jump to the catch block.  
				raiserror ('exist data in database', -- message text.  
						   16, -- severity.  
						   1 -- state.  
						   ); 
			if not exists (select @vref_no from dbo.CustomerData where ReferenceID = @vref_no)
				-- raiserror with severity 11-19 will cause execution to   
				-- jump to the catch block.  
				raiserror ('Invalid AccountNo.', -- message text.  
						   16, -- severity.  
						   1 -- state.  
						   ); 

			insert into dbo.customeracceptedconsentform_log(id,account_no,consent_form_id,consent_date,is_use,system_name,system_ip_address,customer_ip_address,mac_address,remark,update_flag,product_name,ref_no)
			values(@vtransaction_id,ltrim(rtrim(upper(@paccount_no))),upper(@pconsent_form_id),getdate(),'Y',upper(@psystem_name),@psystem_ip_address,@pcustomer_ip_address,@pmac_address,@premark,'A',upper(@pproduct_name),upper(@vref_no))

			insert into dbo.customeracceptedconsentform(id,account_no,consent_form_id,consent_date,is_use,system_name,system_ip_address,customer_ip_address,mac_address,remark,update_flag,product_name,ref_no)
			select id,account_no,consent_form_id,consent_date,is_use,system_name,system_ip_address,customer_ip_address,mac_address,remark,update_flag,product_name,ref_no
			from dbo.customeracceptedconsentform_log
			where id=@vtransaction_id


		commit
		select 0 errornumber,'' errormessage

	end try  
	begin catch  
		rollback
		select   
			error_number() as errornumber  
			,error_message() as errormessage;  
	end catch;  
end
go

-- add new customer_accepted_consent_detail
if exists (
        select type_desc, type
        from sys.procedures with(nolock)
        where name = 'udp_cnms_save_customer_accepted_consent_detail'
            and type = 'p'
      )
     drop procedure dbo.udp_cnms_save_customer_accepted_consent_detail
go

--  exec udp_cnms_save_customer_accepted_consent_detail 'test001','F73D49FD-EB8D-44EC-AC6D-D702F8327133','Y'
create proc dbo.udp_cnms_save_customer_accepted_consent_detail
		@paccount_no varchar(10) ,
		@pconsent_id  uniqueidentifier,
		@pis_accepted char(1)
as
begin 

	begin try  
		begin transaction
			if(len(@paccount_no)>5)
				set @paccount_no = SUBSTRING(@paccount_no,len(@paccount_no)-4,len(@paccount_no))
			declare @vtransaction_id uniqueidentifier = newid()
			declare @vis_accepted char(1)
			-- check valid data
			if exists (select consent_id from dbo.CustomerAcceptedConsentDetail where consent_id = @pconsent_id and account_no = @paccount_no)

				raiserror ('exist data in database', -- message text.  
						   16, -- severity.  
						   1 -- state.  
						   ); 

			-- check valid consent_id
			if not exists (select consent_id from dbo.ConsentDetail where consent_id = @pconsent_id )
				raiserror ('Invalid consent id.', -- message text.  
						   16, -- severity.  
						   1 -- state.  
						   );

			if(@pis_accepted ='Y'or @pis_accepted ='y')
				set @vis_accepted = 'Y'
			else
				set @vis_accepted = 'N'

			insert into dbo.CustomerAcceptedConsentDetail_Log(id,account_no,consent_id,is_accepted,create_datetime,update_flag)
			values(@vtransaction_id,ltrim(rtrim(upper(@paccount_no))),upper(@pconsent_id),upper(@vis_accepted),GETDATE(),'A')

			insert into dbo.CustomerAcceptedConsentDetail
			select id,account_no,consent_id,is_accepted,create_datetime,update_flag
			from dbo.CustomerAcceptedConsentDetail_Log
			where id=@vtransaction_id


		commit
		select 0 errornumber,'' errormessage

	end try  
	begin catch  
		rollback
		select   
			error_number() as errornumber  
			,error_message() as errormessage;  
	end catch;  
end
go

-- delete user_accepted_consent
if exists (
        select type_desc, type
        from sys.procedures with(nolock)
        where name = 'udp_cnms_delete_customer_accepted_consent'
            and type = 'p'
      )
     drop procedure dbo.udp_cnms_delete_customer_accepted_consent
go

--  exec udp_cnms_delete_customer_accepted_consent '00D0471','6C074227-BE2C-4592-8775-26932ACA5DDE','',''
--  exec udp_cnms_delete_customer_accepted_consent '00799','EQUITY'
create proc dbo.udp_cnms_delete_customer_accepted_consent
		@paccount_no varchar(10),
		@pproduct_name  varchar(20),
		@psystem_name varchar(50),
		@puser_name varchar(50),
		@premark varchar(255)
as
begin 

	begin try  
		if(len(@paccount_no)>5)
				set @paccount_no = SUBSTRING(@paccount_no,len(@paccount_no)-4,len(@paccount_no))
		begin transaction
			declare @tb_tmp_account table(
			 tmp_accountno varchar(10),
			 tmp_refno varchar(20)
			 )
			 declare @vref_no varchar(20)

			 select @vref_no = ReferenceID from CustomerData where AccountNo = @paccount_no

			set @psystem_name = isnull(@psystem_name,'')
			set @puser_name = isnull(@puser_name,'')
			set @premark = isnull(@premark,'')

			insert into @tb_tmp_account
			select AccountNo, ReferenceID 
			from CustomerData a
			inner join CustomerAcceptedConsentForm b on b.ref_no = a.ReferenceID and b.product_name = @pproduct_name
			where ReferenceID = @vref_no	



			-- check valid data
			if @pproduct_name is null
				raiserror ('Invalid Product name', -- message text.  
						   16, -- severity.  
						   1 -- state.  
						   ); 

			if not exists (select ref_no from dbo.CustomerAcceptedConsentForm where ref_no in (select tmp_refno from @tb_tmp_account) )

				raiserror ('consent form not exist in database', -- message text.  
						   16, -- severity.  
						   1 -- state.  
						   ); 




			---  delete customer consent form
			update dbo.CustomerAcceptedConsentForm
			set update_flag='D'
			,remark = 'Delete'+@paccount_no+' by '+@psystem_name+':'+@puser_name+'@'+convert(varchar,getdate(),113)+':'+@premark
			where ref_no in(select distinct tmp_refno from @tb_tmp_account) and product_name = @pproduct_name

			--select * from CustomerAcceptedConsentForm where ref_no in(select distinct ref_no from @tb_tmp_account) and product_name = @pproduct_name


			insert into CustomerAcceptedConsentForm_Log
			select * 
			from CustomerAcceptedConsentForm 
			where ref_no in(select distinct tmp_refno from @tb_tmp_account) and product_name = @pproduct_name

			delete from CustomerAcceptedConsentForm
			where ref_no in(select distinct tmp_refno from @tb_tmp_account) and product_name = @pproduct_name
			
			---  delete customer consent detail
			update dbo.customerAcceptedConsentDetail
			set update_flag='D'
			where  account_no  in (select tmp_accountno from @tb_tmp_account) 

			insert into dbo.CustomerAcceptedConsentDetail_Log
			select *
			from dbo.CustomerAcceptedConsentDetail
			where  account_no  in (select tmp_accountno from @tb_tmp_account) 

			delete from dbo.CustomerAcceptedConsentDetail
			where  account_no  in (select tmp_accountno from @tb_tmp_account) 

		commit
		select 0 errornumber,'' errormessage

	end try  
	begin catch  
		rollback
		select   
			error_number() as errornumber  
			,error_message() as errormessage;  
	end catch;  
end
go

if exists (
        select type_desc, type
        from sys.procedures with(nolock)
        where name = 'udp_cnms_update_customer_data'
            and type = 'p'
      )
     drop procedure dbo.udp_cnms_update_customer_data
go
-- exec udp_cnms_update_customer_data
create proc dbo.udp_cnms_update_customer_data
as
begin 

	begin try  
		begin transaction

		delete from CustomerData

		insert into CustomerData
		select a.AccountNo,a.ReferenceID,ltrim(rtrim(b.NameEn)),ltrim(rtrim(b.NameTh))
		from Backoffice.dbo.SBAS_HLDMAS a
			inner join Backoffice.dbo.SBAS_SECMAS  b on a.AccountNo = b.AccountNo

		commit
		select 0 errornumber,'' errormessage

	end try  
	begin catch  
		rollback
		select   
			error_number() as errornumber  
			,error_message() as errormessage;  
	end catch;  
end
go
        