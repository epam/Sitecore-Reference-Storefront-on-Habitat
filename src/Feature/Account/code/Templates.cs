namespace Sitecore.Feature.Account
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct AccountManager
        {
            public static readonly ID ID = new ID("{6C0FC189-1CDA-413B-B462-14E5518E1D3F}");

            public struct Fields
            {
                public static readonly ID DateLabel = new ID("{3B669CE1-FB96-4CA5-BF57-8F958DABCF7B}");
                public static readonly ID OrderNumberLabel = new ID("{EE964941-5302-4603-B24F-0437CEB488FA}");
                public static readonly ID StatusLabel = new ID("{39033A36-1D97-43B0-A7E6-199D0F436C88}");
                public static readonly ID ProfileLabel = new ID("{30F5A63A-4816-45C0-8491-D962DA29CDF5}");
                public static readonly ID NameLabel = new ID("{20D39BB7-A9C3-4AF4-89F6-32FDBDE25666}");
                public static readonly ID AddressLabel = new ID("{85ECC994-FE65-4E56-80C4-4D4D59647691}");
                public static readonly ID CardNumberLabel = new ID("{5236DAD4-702D-4CEE-888F-51E7953A89CE}");
                public static readonly ID ViewAllOrdersLabel = new ID("{4FDED9AC-57F7-493C-9902-2F0ED5E1CD37}");
                public static readonly ID AddNewAddressLabel = new ID("{82A3D299-A37D-46FC-9ACA-289FCA0F4C6B}");
                public static readonly ID ViewAllLabel = new ID("{674E521C-D5E6-4EB7-BE33-8724E688766E}");
                public static readonly ID ViewDetailsLabel = new ID("{F82B1DBA-709B-4106-9634-65365E9B1749}");
                public static readonly ID PrimaryAddressLabel = new ID("{42074D5D-2228-4EA0-B621-5C7F852EE4E6}");
                public static readonly ID ChangePasswordLabel = new ID("{BCB8BFEE-F776-449B-907D-EDB7CFC29108}");
                public static readonly ID WishlistNameLabel = new ID("{7096038A-667B-4D2F-8829-1E4A715D49BF}");
                public static readonly ID CreateNewWishlistLabel = new ID("{4CD14B92-6943-4378-AF90-BCCA74AD6F33}");

                public static readonly ID OrdersTitle = new ID("{1CDCBA80-CDA3-4996-88AE-20F49807B25C}");
                public static readonly ID AccountProfileTitle = new ID("{62788F20-BADD-4A38-8EDC-3B23C786D2C9}");
                public static readonly ID WishListTitle = new ID("{AEF06D94-A267-429F-9FF1-157BE7410D13}");
                public static readonly ID AddressBookTitle = new ID("{19666030-1D0E-4423-A7F6-8452C8C244F1}");
                public static readonly ID LoyaltyTitle = new ID("{FC33E2CE-B900-4603-B406-1748E5FD07E9}");

                public static readonly ID OrderTableText = new ID("{03D294BC-6503-4239-B1F3-F13C0E87D705}");
                public static readonly ID AccountProfileText = new ID("{1B2E5B5E-251F-4E3C-8288-432E4902E52B}");
                public static readonly ID WishListEmptyText = new ID("{F1F07E2D-E278-44D9-8426-BC4595E297E6}");
                public static readonly ID LoyaltyEmptyText = new ID("{4CFFC76A-61CC-420C-9E58-40CAFF8D0ED3}");
                public static readonly ID AddressBookEmptyText = new ID("{F6B4CA14-0A00-4A7F-88BE-684961D050B8}");

                public static readonly ID JoiningLoyaltyProgramButton = new ID("{5F4F242E-F947-4027-945B-D3DA9CA2D7F3}");
                public static readonly ID SavingButton = new ID("{67A50DC1-BBC7-4394-A142-8A0ADD91F050}");
                public static readonly ID CreateNewWishlistButton = new ID("{4CBE2BB0-D74C-451D-9444-FD1AEF7490C8}");
                public static readonly ID JoinLoyaltyProgramButton = new ID("{2E90D854-47D3-4485-9C77-13F60CBC8F2C}");
                public static readonly ID SaveChangesButton = new ID("{57AC22D6-7ABD-4A4E-95F7-98AFB47953AB}");
                public static readonly ID CloseButton = new ID("{F0ED76E4-62D9-4453-80E3-189FE2772287}");
            }
        }

        public struct Register
        {
            public static readonly ID ID = new ID("{E78B0317-1F4A-4DAB-A507-5B6BBDF7F41C}");

            public struct Fields
            {
                public static readonly ID Registering = new ID("{25654646-90C4-487F-96B2-9AB21BFDBFEB}");
                public static readonly ID Email = new ID("{D788A7E2-00D1-44C0-96A1-993A51FD274C}");
                public static readonly ID FirstName = new ID("{0754EF47-267D-4390-A004-A28BADEF4D0B}");
                public static readonly ID LastName = new ID("{B7938EC0-1F55-45DE-8010-C59F52EF43E9}");
                public static readonly ID Password = new ID("{C025EE06-D9F1-45A3-9BBE-B849C24EA25F}");
                public static readonly ID PasswordAgain = new ID("{96760D29-01C8-4158-B80D-8AE8B3427EF3}");
                public static readonly ID CreateUser = new ID("{272FD624-6448-464B-AD8F-ED706C580F1D}");
                public static readonly ID Cancel = new ID("{04FED636-94A9-47E4-92CD-A7F92BEFB811}");
                public static readonly ID FillFormMessage = new ID("{E066EB82-1F62-4E92-A749-2C885A6FBCF4}");
                public static readonly ID FacebookButton = new ID("{0F00BF99-7F90-4FCA-B56C-B171F305477C}");
                public static readonly ID FacebookText = new ID("{CB93092D-1DD9-4BC7-B30C-BAC55E1B6E26}");

                public static readonly ID EmailAddressPlaceholder = new ID("{6F1B8496-8581-43BF-93B2-983CC992BBE7}");
                public static readonly ID FirstNamePlaceholder = new ID("{601747AC-BBA2-4D26-940A-990EBD71F2E7}");
                public static readonly ID LastNamePlaceholder = new ID("{6DFEAA2A-D395-4D6B-9A28-BAA067234C74}");
                public static readonly ID PasswordPlaceholder = new ID("{11E1495C-FC5F-40E7-A8FC-0AEC024D4D66}");

                public static readonly ID EmailMissingMessage = new ID("{B65DF6CB-E4D5-4B11-9DA5-1D1E411EEBC1}");
                public static readonly ID PasswordMissingMessage = new ID("{6FFD7D04-6ADD-4608-9FEF-304CC12DDE36}");
                public static readonly ID PasswordLengthMessage = new ID("{2EF94CAE-2FC4-4936-80E5-CC14D680CC5C}");
                public static readonly ID PasswordsDoNotMatchMessage = new ID("{C4A8E07E-4CD4-4B78-8C1A-68D327DCEBBD}");

            }
        }

        public struct EditProfile
        {
            public static readonly ID ID = new ID("{C9DCDE83-6B72-4B6D-A4C6-2729835BC44A}");

            public struct Fields
            {
                public static readonly ID FirstName = new ID("{13C97726-033B-4E1A-9837-2F9FB2DF7C20}");
                public static readonly ID Surname = new ID("{3C9C1491-4030-454D-9DEC-EA30510B5B43}");
                public static readonly ID Email = new ID("{7413052F-1CFF-4B3D-862E-DDA7DA617E48}");
                public static readonly ID RepeatEmail = new ID("{E93C0A73-85EE-4968-9FE9-94DDEAE5297A}");
                public static readonly ID SaveChangesButton = new ID("{C003AF96-E780-4E0D-B45C-1C45E5E8F912}");
                public static readonly ID CancelButton = new ID("{4328139F-4801-4930-AB9E-510A69A62473}");
                public static readonly ID BackToMyAccount = new ID("{A4115EA4-306F-43E5-B4D7-6346A2CBEB2C}");
                public static readonly ID SavingChangesButton = new ID("{A1E4E7F0-F89D-4682-8B5D-06B58CC76885}");
                public static readonly ID TelephoneNumber = new ID("{6F00D900-B23C-4F3C-AFCF-A03FB5B0B6C5}");

                public static readonly ID TelephoneNumberPlaceholder = new ID("{2EF9BDB1-9827-40C5-8975-3409196963D1}");
                public static readonly ID PasswordPlaceholder = new ID("{E8F1A5FF-D477-4A85-9312-C9939EA8DA3B}");
                public static readonly ID EmailPlaceholder = new ID("{B3EA88CE-8B8F-49EC-9239-1F00812EF4EF}");
                public static readonly ID FirstNamePlaceholder = new ID("{E3707BA5-AF84-4ABE-A7BA-D8C24C37C6AA}");
                public static readonly ID LastNamePlaceholder = new ID("{A341E52D-4255-489F-AD23-5FCA201F77F9}");

                public static readonly ID FirstNameRequiredMessage = new ID("{8B121B2F-9782-4C87-BC05-7825C940AAAB}");
                public static readonly ID LastNameRequiredMessage = new ID("{109A8E80-C052-4F8E-8A7E-5878CF467E71}");
                public static readonly ID EmailRequiredMessage = new ID("{0C1B7DAD-D2D0-4640-AD1E-EBE31F159D51}");
                public static readonly ID TelephoneNumberRequiredMessage = new ID("{460C9B9E-8723-49FF-8DBA-03AB18379DA6}");
            }
        }

        public struct Login
        {
            public static readonly ID ID = new ID("{F7134EB0-A57C-40E4-910E-5ABE1BF653EC}");

            public struct Fields
            {
                public static readonly ID SigningButton = new ID("{9F832330-D023-478C-93FC-220C1129C714}");
                public static readonly ID CustomerMessage1 = new ID("{C38AA797-891E-40DE-8B95-D1D502CD3261}");
                public static readonly ID CustomerMessage2 = new ID("{F5ED105B-6576-4A4C-B5BD-61D69C4B4C51}");
                public static readonly ID Email = new ID("{147CFB98-67FD-459E-8A8E-4A8092075350}");
                public static readonly ID Password = new ID("{32D9809A-9D32-4F3C-BC15-317BF66DD194}");
                public static readonly ID SignInButton = new ID("{BDBDC686-D372-484A-B650-B6FD48BAF3A3}");
                public static readonly ID RegisterNewAccount = new ID("{8408455C-FA71-4AF1-976F-59D1F70169C2}");
                public static readonly ID GuestCheckoutButton = new ID("{A5461769-1021-4358-8395-108D39606CB0}");
                public static readonly ID FacebookButton = new ID("{32230721-8845-4A48-8020-5C5B3D3C9569}");
                public static readonly ID FacebookText = new ID("{B2AFD9F7-2E3D-4EAA-A0CF-A82D1C308EC1}");
                public static readonly ID ForgotPassword = new ID("{FD6DE0AA-3C18-41DF-B3B1-55ED5F16A46E}");

                public static readonly ID EmailAddressPlaceholder = new ID("{769DCA41-4C8A-4F0C-A078-82C1FF929C59}");
                public static readonly ID PasswordPlaceholder = new ID("{C3A24E20-8391-4568-AFFE-AB48DF628FE5}");

                public static readonly ID EmailMissingMessage = new ID("{CC1189E1-1FDC-4396-BE4D-C5A5B107B7EB}");
                public static readonly ID PasswordMissingMessage = new ID("{2DD792A4-EE8B-4227-8580-F12071658C89}");
            }
        }

        public struct AddressBook
        {
            public static readonly ID ID = new ID("{EFC3B5EE-E484-44B3-8453-729268BA8C53}");

            public struct Fields
            {
                public static readonly ID ChangeAddressLabel = new ID("{90C4D9B9-8620-450A-A674-57EB110EF818}");
                public static readonly ID AddressLabel = new ID("{1D588062-5217-4496-B325-94B0981369CD}");
                public static readonly ID CityLabel = new ID("{D91AB418-C356-4FB6-89DE-5EA2CD7C150B}");
                public static readonly ID CountryLabel = new ID("{77BE2267-2102-4BF0-8CA0-C295677E855C}");
                public static readonly ID StateLabel = new ID("{388690DA-0E7F-4061-9237-E6CC1A3B7101}");
                public static readonly ID ZipCodeLabel = new ID("{541542D6-BDF5-42D4-B20E-021C0287A163}");
                public static readonly ID SetAsPrimaryLabel = new ID("{7FD53F4B-D6FB-4F31-AD17-1FA6DED84E3A}");
                public static readonly ID BackToMyAccountLabel = new ID("{654B1DD8-9B4F-4ACA-ABAE-82F816EE0F70}");
                public static readonly ID SavingLabel = new ID("{80A14700-60E0-487D-8B0C-9CCF8B26768E}");
                public static readonly ID DeletingLabel = new ID("{B6D5EA80-1003-4A93-8437-3D334DDE9AE3}");
                public static readonly ID SelectLabel = new ID("{CAA5DC96-A392-4E4A-878A-58F0414E533F}");
                public static readonly ID NewAddressLabel = new ID("{8881DF15-8613-4CC3-9B5C-09235413D8B0}");
                public static readonly ID NameLabel = new ID("{E16E9B13-D388-419E-B8BD-48C71EF0D3BB}");

                public static readonly ID SaveChangesButton = new ID("{FC30AAD9-0050-42CF-B721-0523072FB812}");
                public static readonly ID CancelButton = new ID("{1BD50106-1AA3-4393-BC2D-D2170F8EAD36}");
                public static readonly ID DeleteAddressButton = new ID("{9AF5F65A-25A9-42E2-BC39-3C29ADBBBA94}");

                public static readonly ID AddressesEmptyText = new ID("{F085C466-000A-40E4-A119-88C3534D9CE3}");
            }
        }

        public struct ChangePassword
        {
            public static readonly ID ID = new ID("{FFC27882-C4F0-4819-92AF-C4DDF98096FC}");

            public struct Fields
            {
                public static readonly ID NewPassword = new ID("{727FFCA2-41BC-4CB2-BD07-328A2516FA67}");
                public static readonly ID RepeatPassword = new ID("{18757981-E7F6-4AB1-95E6-4CA68B65D788}");
                public static readonly ID CurrentPassword = new ID("{2CE72B31-F944-4A0B-9EA8-23AADEAD0C31}");
                public static readonly ID SaveChangesButton = new ID("{C8E323D4-3AC3-49E1-8322-619F0168D4FE}");
                public static readonly ID CancelButton = new ID("{CD38A0FF-D6FA-4F14-810F-B50778663E10}");
                public static readonly ID BackToMyAccount = new ID("{2FA419B5-DA10-4B79-AFFC-F552628C893B}");
                public static readonly ID SavingChangesButton = new ID("{8443FF6D-0783-45FD-8657-07BEFFA9A0B0}");

                public static readonly ID CurrentPasswordRequiredMessage = new ID("{2998CB6A-5FD0-4D1E-96EF-5F3E148F8C40}");
                public static readonly ID PasswordAndConfirmationMustMatchMessage = new ID("{FD1AD1EB-CA9C-4808-93EF-09A2A28FF8C6}");
                public static readonly ID ThePasswordLengthMessage = new ID("{EFD2B75F-7354-4EB4-86CD-6D13D911B06C}");
            }
        }

        public struct Order
        {
            public static readonly ID ID = new ID("{95D0E2E0-42F9-437D-A7CD-DB02F3CA0593}");

            public struct Fields
            {
                public static readonly ID OrderNumberLabel = new ID("{9E13E4AD-A169-42EF-BB85-F8257A080B00}");
                public static readonly ID OrderDateLabel = new ID("{E4D24DDF-4336-4A36-8252-6DB88EF93FE9}");
                public static readonly ID OrderStatusLabel = new ID("{B7BC66A7-2FBD-4A75-BAE1-B4255C4896BF}");
                public static readonly ID ColorLabel = new ID("{8809E952-4681-4F4B-AEDE-199F27929ECF}");
                public static readonly ID DeliveryLabel = new ID("{DB9C6C0C-9A3E-45A3-A14B-5921F37BFD69}");
                public static readonly ID SavingsLabel = new ID("{258B65F7-553D-4B86-9642-9F1AD528EE55}");
                public static readonly ID PromotionCodesLabel = new ID("{1FAF7AFC-168A-4793-97EA-42C28AF2AAF9}");
                public static readonly ID LoyaltyCardLabel = new ID("{AC13E44D-DF37-4619-A140-225553293FE7}");
                public static readonly ID SubtotalLabel = new ID("{3564A1F4-4988-47B1-A8BF-FC00969E22D1}");
                public static readonly ID ShippingTotalLabel = new ID("{06126E2E-1A2F-42C4-8B74-CD7AA7E6C4AF}");
                public static readonly ID TaxTotalLabel = new ID("{7EC57576-A58A-465D-9E5E-EBFD9EB94980}");
                public static readonly ID OrderTotalLabel = new ID("{BD92CEE4-B867-4D67-9708-3F0B6D33BF6D}");
                public static readonly ID ShippingAddressLabel = new ID("{0DD06FFB-7B7A-4B83-92BC-C92664CD10B0}");
                public static readonly ID BillingAddressLabel = new ID("{97D77995-A3BA-4C22-AED5-094174626610}");
                public static readonly ID ShippingMethodLabel = new ID("{C97AFCFE-8170-4E47-A7A6-0775B7E94E14}");
                public static readonly ID PaymentsLabel = new ID("{577520DE-AA1E-46FE-8AEF-872E19301CFC}");
                public static readonly ID ViewAllOrdersLabel = new ID("{AE228CB9-9406-4ED4-939D-ED4A7DF09652}");
                public static readonly ID DiscountLabel = new ID("{8FB602A4-1F4C-4D47-929E-5507E697A555}");

                public static readonly ID YourProductsHeader = new ID("{2846E9BF-663C-48E3-92C7-75B77D8ADDEF}");
                public static readonly ID LoyaltyRewardsHeader = new ID("{F53A75B0-CB77-42DE-9976-94CAB83ED6F0}");
            }
        }

        public struct OrderList
        {
            public static readonly ID ID = new ID("{7BA48106-EFDB-471D-A736-DBC9D0AD5DC5}");

            public struct Fields
            {
                public static readonly ID BackToMyAccountLabel = new ID("{36DF18A7-4794-4C9C-BC61-EA515494F82D}");
                public static readonly ID OrderNumberLabel = new ID("{B5CB89F5-AD33-4D61-9C99-BE47B0750D56}");
                public static readonly ID OrderDateLabel = new ID("{F4E176DC-4D51-408F-9CB3-6C22381E55B5}");
                public static readonly ID OrderStatusLabel = new ID("{50067D29-1AF0-4AF8-B051-A507C0FA2E2A}");
            }
        }

        public struct ForgotPassword
        {
            public static readonly ID ID = new ID("{985C9342-C79C-47D2-A3F7-BDC6DCCC0BA7}");

            public struct Fields
            {
                public static readonly ID Submitting = new ID("{980BBF2B-C208-4A75-B9CA-E1C51B6C45D0}");
                public static readonly ID Cancel = new ID("{CD8C9707-FE7A-4D3E-912C-95F9F2D0007D}");
                public static readonly ID Email = new ID("{ED62D6A1-7371-476A-93D9-C95080613D47}");
                public static readonly ID FillFormMessage = new ID("{A3995F9E-3242-4F02-84E7-BE95BB0CFCE1}");
                public static readonly ID Submit = new ID("{42ABB57F-EC16-4278-A08E-AA13169ABD8B}");

                public static readonly ID EmailAddressPlaceholder = new ID("{FC59D69E-66FB-42DE-8621-54C1966A745A}");

                public static readonly ID EmailMissingMessage = new ID("{A48DCFF4-5AD2-4D4E-8E61-4E4C538770E7}");
            }
        }

        public struct EmailSender
        {
            public static readonly ID ID = new ID("{3D118033-9A3E-4BE7-9B45-6D192FAE45EA}");

            public struct Fields
            {
                public static readonly ID Subject = new ID("{F092096E-BFFB-4834-ADB9-66480ED5CF81}");
                public static readonly ID Body = new ID("{10624C02-1F6E-48C4-8ADE-90CDFCF64C22}");
            }
        }

        public struct ConfirmForgotPassword
        {
            public static readonly ID ID = new ID("{8BF33F89-5988-4294-AE10-D78F811CAD52}");

            public struct Fields
            {
                public static readonly ID Email = new ID("{F1ED673E-D262-437C-B074-89FE2F11E785}");
                public static readonly ID BackToShoppingButton = new ID("{FE750980-1773-482F-92F2-99CC4CF5F117}");
                public static readonly ID FillFormMessage = new ID("{7688A550-3C37-4E2B-A710-A8CF34726CEC}");
            }
        }
    }
}
