namespace Sitecore.Feature.Cart
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct AddGiftCardToCart
        {
            public static readonly ID ID = new ID("{CCF3975B-D3AD-41E0-AC52-1C9126974078}");

            public struct Fields
            {
                public static readonly ID AddToCartButton = new ID("{0636163A-E6CD-416A-91DC-8B703E0054D5}");
                public static readonly ID AddingToCartLabel = new ID("{861390E5-5956-46DC-99AE-F70B8BDAE87E}");
                public static readonly ID GiftCardAmountLabel = new ID("{2D951E0E-D349-4541-BE10-45214D43A1B5}");
                public static readonly ID QuantityLabel = new ID("{27806479-AA11-4372-9D0C-D50AD36B1008}");
            }
        }

        public struct AddToCart
        {
            public static readonly ID ID = new ID("{6B3C9998-AB94-4A3A-A44F-D9E2DB12739D}");

            public struct Fields
            {
                public static readonly ID QuantityLabel = new ID("{524BCD07-A070-4B7B-9BFB-9D3B84ECE74B}");
                public static readonly ID AddingToCartLabel = new ID("{CCC6D8B6-A3CE-4E5A-B70A-297E62666ACE}");
                public static readonly ID AddToCartLabel = new ID("{FA500705-683D-4F84-AFF5-29585F782D72}");
            }
        }
        
        public struct MiniCart
        {
            public static readonly ID ID = new ID("{51E92044-48FB-4CED-BAF4-94F34674DFDB}");

            public struct Fields
            {
                public static readonly ID Cart = new ID("{7D5F847F-09C2-499F-BF59-5FB07829AD82}");
                public static readonly ID Quantity = new ID("{DFE0F97D-5A01-4C23-9037-4637EA48B1A5}");
                public static readonly ID Price = new ID("{BE9EBF28-579F-4B25-A851-85EA2DDFB719}");
                public static readonly ID Title = new ID("{E9C22DF9-5E98-4CFB-8A76-34521297332D}");
                public static readonly ID Total = new ID("{E5827172-F48C-4E97-8D48-DD546F6C6318}");
                public static readonly ID ViewCartLabel = new ID("{67021CB8-3C03-4D60-AA4C-76024E6463F9}");
                public static readonly ID CheckoutLabel = new ID("{C348822F-F374-40A7-8CEB-58240DB301F5}");
            }
        }

        public struct ShoppingCart
        {
            public static readonly ID ID = new ID("{F8CDA7F6-C0A0-4779-8504-A1A95DB1DFB3}");

            public struct Fields
            {
                public static readonly ID ProductDetailsLabel = new ID("{BCAE4882-2C82-42D7-8B33-D269CB521E36}");
                public static readonly ID UnitPriceLabel = new ID("{03090E3B-94DA-461D-9419-C9235FCE5C1B}");
                public static readonly ID QuantityLabel = new ID("{ED2A6656-70D9-4B47-91C4-B2B3E574C6C6}");
                public static readonly ID TotalLabel = new ID("{361B56FE-46EE-4E86-9A83-5F7A1127E846}");
                public static readonly ID PaymentTotalLabel = new ID("{03972573-F2E1-4750-973C-ABE93FAE5F81}");
                public static readonly ID VATLabel = new ID("{EC26E453-F364-4070-B697-70C5C6D5ECAF}");
                public static readonly ID OrderTotalLabel = new ID("{6C2B407A-476F-4B29-80B7-78526FF43645}");
                public static readonly ID TotalSavingsLabel = new ID("{782EC575-8B6D-4F75-B020-21126AE25A21}");
                public static readonly ID OrderTotalHeaderLabel = new ID("{3E503A8F-AD03-464E-B61D-09CC8D6E79F7}");
                public static readonly ID ColorLabel = new ID("{E5337D27-918E-4DC7-955A-9185314C6615}");
                public static readonly ID DiscountLabel = new ID("{2C2FA18D-775B-4E00-80BB-20022B19402B}");
                public static readonly ID SavingsLabel = new ID("{F1CDCC4B-3183-4465-BFA7-3DAE152CAFA9}");
                public static readonly ID EnterCouponCodeMessage = new ID("{71834529-FFDC-4D99-8203-86576CC04F33}");
                public static readonly ID PromoCodeLabel = new ID("{00A44708-3F62-40DA-BD5C-96EAFD85CAC2}");
                public static readonly ID AddPromoCodeButton = new ID("{88C87F20-8237-4E2F-87F7-2D6B6EA43331}");
                public static readonly ID PromoCodeListLabel = new ID("{4FE90BE9-09E0-4915-881E-0DEF17A3F3A0}");
                public static readonly ID RemoveButton = new ID("{EB60A902-0075-4D80-9649-0B0A6AFEE633}");
                public static readonly ID AddingPromoCodeButton = new ID("{7774845A-2602-442F-8741-2B34C1623100}");
                public static readonly ID ShippingLabel = new ID("{10C5A9BD-14ED-49F6-BA5C-70F4BD0BD4A1}");

                public static readonly ID BackToShoppingButton = new ID("{ED153781-E66A-49FA-8149-1EE7C6FC5452}");
                public static readonly ID CheckoutButton = new ID("{D8221196-CA94-4D7C-A67D-A9C5E0ED496C}");
            }
        }

        public struct FirstCheckoutStep
        {
            public static readonly ID ID = new ID("{3AFD5670-142F-4BDE-8118-EAEFB0BEC187}");

            public struct Fields
            {
                public static readonly ID ProcessingLabel = new ID("{9B303130-F463-4DCE-A25F-CA1877C950EB}");
                public static readonly ID DeliveryInformationLabel = new ID("{F93E527F-F7D8-4DD7-81C1-1D78258BEA60}");
                public static readonly ID BillingInformationLabel = new ID("{2F7B36FD-C055-4CE3-8096-A196E209ED22}");
                public static readonly ID ConfirmationLabel = new ID("{DC264DC6-6E66-44BE-A58E-0D76F7A00614}");

                public static readonly ID CartButtonContinue = new ID("{C8DBEFC1-E3A6-4839-8556-14D2CCFD3AB4}");
            }
        }

        public struct DeliveryCheckoutStep
        {
            public static readonly ID ID = new ID("{F7BBA2D8-A212-457D-A143-62DBCFA6FB95}");

            public struct Fields
            {
                public static readonly ID AvailableShippingOptionsLabel = new ID("{971AE443-6884-4C59-80A3-0B325C0524B9}");
                public static readonly ID DeliveryPreference = new ID("{AEE70C35-610A-4B1C-B03D-5E0A67195B3D}");
                public static readonly ID PickUpAtStoreHeader = new ID("{6E1F8154-749A-49BB-8845-E8B352CC267C}");
                public static readonly ID SelectDeliveryOptionsHeader = new ID("{8614B786-115C-4CD8-A66B-BB80EB89B6EB}");
                public static readonly ID SendByEmailHeader = new ID("{DED5A5D3-9A5B-4FB4-84FA-6BCD9B69FF00}");
                public static readonly ID NextButtonLabel = new ID("{B52F3D52-C8FC-48A6-ACDF-56A54653B886}");
                public static readonly ID PrevButtonLabel = new ID("{349BDE3B-2BA8-49FE-9A15-0FFE142C32CA}");
                public static readonly ID ShipThisItemHeader = new ID("{6E4A71EA-31C3-4FAF-890C-E61372A46A96}");

                public static readonly ID CloseDeliveryInfoLabel = new ID("{3F864C66-8A6A-447E-A569-B17C106ADD9C}");
                public static readonly ID CloseStoresLabel = new ID("{B833244E-882B-4121-ACD0-260417C737F3}");
                public static readonly ID ColorLabel = new ID("{58240B0F-BDB7-4554-93E9-9C4D4D9CED35}");
                public static readonly ID DeliveryOptionLabel = new ID("{0B634977-6D1F-4C83-959C-13D666862734}");
                public static readonly ID DeliverySelectedLabel = new ID("{271E224E-0704-409D-A1A7-49244EB693B3}");
                public static readonly ID FindStoreLabel = new ID("{8EFE3947-EE9F-4A03-BBC7-ABEA5F3C09C7}");
                public static readonly ID OpenDeliveryInfoLabel = new ID("{E234D011-7A30-4F31-96E2-E3787D9BFD1F}");
                public static readonly ID RecipientEmailLabel = new ID("{ADC98EE6-2A31-4600-BF2C-9FFC13086127}");
                public static readonly ID SendToMeLabel = new ID("{BE3F4890-7E2A-48D6-AABF-0F10DB80EA1A}");
                public static readonly ID WriteMessageLabel = new ID("{14C82178-DCBE-4C18-A231-72090B19B1F5}");
                public static readonly ID ChooseAddressLabel = new ID("{44D81C03-1352-48F6-86B4-7C4E04155C2D}");
                public static readonly ID ShipAllItemsLabel = new ID("{DB8B54AF-55D4-4AB0-8141-FFD91E7B08F8}");
                public static readonly ID ShippingAddressLabel = new ID("{3115D33A-8132-4A45-B82D-26C6B76CFCC7}");
                public static readonly ID ChooseAddressBookLabel = new ID("{D81B5D09-610A-4782-9401-D9543AAA4795}");
                public static readonly ID ShippingNameLabel = new ID("{70E55EFB-678B-4BEC-A608-B0E632334974}");
                public static readonly ID CityLabel = new ID("{3D81FAF6-A20E-4969-B6F6-00A5E0B16314}");
                public static readonly ID CountryLabel = new ID("{81086E46-C678-4336-8588-844B3D4910CF}");
                public static readonly ID StateLabel = new ID("{3AE814A0-7822-4E8F-9BCC-9DB5E0FFEA34}");
                public static readonly ID AddressLabel = new ID("{6D7CCF0C-8DD2-4768-A76E-64DBB992ED07}");
                public static readonly ID ZipcodeLabel = new ID("{FD8346E1-6FA0-49EF-BBD1-F036BC1C0E14}");
                public static readonly ID ViewButtonLabel = new ID("{72F3481D-D5D3-4EC8-8247-C617D2D68A40}");
                public static readonly ID ViewButtonLoadingLabel = new ID("{11FBC8F4-DDAA-4041-B1A0-1A9523A129EA}");
                public static readonly ID SearchInputPlaceholder = new ID("{D2225B35-6381-4579-AE0A-8AC8585C2D4F}");
                public static readonly ID ChooseOtherAddressLabel = new ID("{8A3ACC81-A34B-49C7-81E9-5ED871CDEE11}");
            }
        }

        public struct BillingCheckoutStep
        {
            public static readonly ID ID = new ID("{055EA966-E09E-4648-B819-37406CFD09E9}");

            public struct Fields
            {
                public static readonly ID ContactInformationHeader = new ID("{6D0902E3-C47C-4DE4-AEB9-2E06F0A8DA0C}");
                public static readonly ID EmailAddressLabel = new ID("{29457D5C-B582-4054-B746-114037C1A33E}");
                public static readonly ID ConfirmEmailAddressLabel = new ID("{2D004CDE-9269-4FA5-B523-F6C12F86CDC8}");
                
                public static readonly ID PaymentOptionsHeader = new ID("{44E61890-4744-49DC-B9B0-8B8D0C8CFE50}");
                public static readonly ID NameonCardLabel = new ID("{3252F170-4CB2-4ABA-8450-77FA42ED6688}");
                public static readonly ID CardNumberLabel = new ID("{3BE0618A-8761-40DF-A27F-2C3C8153C283}");
                public static readonly ID ExpirationDateLabel = new ID("{7FC4ECAA-2AED-4ADE-B9E4-F1265C1E824C}");
                public static readonly ID ExpirationYearLabel = new ID("{B38E1871-9F00-4D57-8419-A67301675B31}");
                public static readonly ID CCIDLabel = new ID("{A9D20A97-C056-430F-B076-368EA2D8AB03}");
                public static readonly ID BillingAddressHeader = new ID("{00249BCB-F270-4EDD-8EC1-3DFE65136314}");
                public static readonly ID BillingNameLabel = new ID("{3BB46433-5D5B-4187-B8CA-99A09117F1CB}");
                public static readonly ID AddressLabel = new ID("{A1237227-60F6-43D5-B9F2-314510E74A03}");
                public static readonly ID CityLabel = new ID("{EA2E2144-3429-4102-BA3B-9702CE0D60BC}");
                public static readonly ID ZipcodeLabel = new ID("{E375ABA1-D631-4C68-B6DA-93E7DDF7CA24}");
                public static readonly ID CountryLabel = new ID("{C84F3978-0801-460F-800D-046A261525B3}");
                public static readonly ID StateLabel = new ID("{A7EB1929-96B8-4670-B46E-D6D1F97E9BD0}");
                public static readonly ID PaypalPassword = new ID("{30E29F51-27EE-44C6-B227-1AEA9BAE36DE}");
                public static readonly ID SignInLabel = new ID("{688BE7CA-7FE3-4B94-A671-FD23B0F763B9}");
                public static readonly ID SameShippingAddress = new ID("{7CC2659F-F7CE-464F-A6EE-E154B14308F8}");

                public static readonly ID CreditCardLabel = new ID("{8B0E5E72-80FA-48C0-9F8E-A7A67B915EBB}");
                public static readonly ID OrderInformationLabel = new ID("{362A19C3-A7E7-414C-9E52-593FADF5A8C7}");
                public static readonly ID PaymentInfoHeader = new ID("{7C91B1E8-B599-4C1D-9EE2-4216E6DEB50F}");
                public static readonly ID ShippingCostLabel = new ID("{4CE9339C-AF1E-49EC-8C55-210E92F3C334}");
                public static readonly ID SubtotalLabel = new ID("{BBB53ED5-6422-43D7-9C16-6B3EC7B172E5}");
                public static readonly ID PaymentTotalLabel = new ID("{86690679-3BE1-42C0-A5DE-96764A815AAC}");
                public static readonly ID TaxesLabel = new ID("{750F2ECB-4EC2-4E33-9786-E950DFF2793E}");
                public static readonly ID SavingsLabel = new ID("{FC614C3E-0667-43B4-9185-739312C9C8BE}");
                public static readonly ID TotalLabel = new ID("{8724FB79-17C0-4602-A0FA-4681927AD591}");
                public static readonly ID ChoosePaymentMethodLabel = new ID("{7C73B197-8938-48F6-A047-29D8443838B0}");
                
                public static readonly ID UpdatingLoyaltyCardButton = new ID("{8A11421A-F926-4E50-A55D-D23E08EC704A}");
                public static readonly ID NextButtonLabel = new ID("{04D47388-9794-44C5-B6D5-78AF663BCF3B}");
                public static readonly ID PrevButtonLabel = new ID("{B1A9C292-A7AA-40D8-86ED-B32840DD8DB1}");
                public static readonly ID GetBalanceButton = new ID("{BCDF63BD-683E-4173-8EF1-C5AD8B6E3CFB}");
                public static readonly ID ApplyAmountButton = new ID("{0BF8DCBF-08F2-4F50-80A1-A603D5342009}");
                public static readonly ID AddLoyaltyCardButton = new ID("{0D330979-115C-4726-85D3-EA00FD4A9CB6}");
                public static readonly ID AddGiftCardButton = new ID("{A3F57B0A-9C20-4EB7-A7DF-8497979252CB}");

                public static readonly ID CardsHeader = new ID("{03097782-47FF-4E76-9F48-0566F6B56949}");
                public static readonly ID ApplyGiftCardLabel = new ID("{37E9811A-B949-4D89-8637-26E8547C7BB8}");
                public static readonly ID SubtitleGiftLabel = new ID("{964A0B0A-6597-4844-A9EA-A03C4B337DFB}");
                public static readonly ID PaymentAmountLabel = new ID("{93E161E7-A2C5-4304-AC45-FDC22FB577B9}");
                public static readonly ID GiftCardLabel = new ID("{162B5D9F-C092-4ECC-B9C2-56A0CCB123E9}");
                public static readonly ID RemoveLabel = new ID("{34BE9733-2FE0-4CB9-B949-8F796574729F}");
                public static readonly ID ApplyLoyaltyCardLabel = new ID("{B3759862-12BF-4972-A814-CBA16257BFB7}");
                public static readonly ID SubtitleLoyaltyLabel = new ID("{191DE834-5438-49EB-B4A7-F1C0D2CE7D6C}");
                public static readonly ID LoyaltyCardLabel = new ID("{82652DC1-D579-4F9B-828C-BD003EEE0C68}");
                public static readonly ID GettingBalanceLabel = new ID("{B0649D6E-7AE1-4473-BEE2-75C31E8B3DDA}");
            }
        }

        public struct ConfirmCheckoutStep
        {
            public static readonly ID ID = new ID("{6458E824-0FE4-44CB-8185-72C2F035A4CE}");

            public struct Fields
            {
                public static readonly ID NextButtonLabel = new ID("{F7164DDB-92FE-4CDF-823D-989C6F90739F}");
                public static readonly ID PrevButtonLabel = new ID("{B62C80F1-DD74-4C89-B510-01EB49D49D95}");

                public static readonly ID ProductDetailsLabel = new ID("{B8C45DEC-446B-445D-A2BD-5A313E6302EF}");
                public static readonly ID UnitPriceLabel = new ID("{BFE51D46-BA02-46B5-B249-E66E0FD93A6D}");
                public static readonly ID QuantityLabel = new ID("{5D91B3A0-8126-44AC-82BE-B52EF403FBBA}");
                public static readonly ID TotalLabel = new ID("{521E1824-D65F-4394-988E-589A6BE42A17}");

                public static readonly ID CreditsHeaderLabel = new ID("{57059042-8C55-4728-9E8A-81BBA8B6D5E2}");
                public static readonly ID ApplyDiscountCreditLabel = new ID("{08F6044B-E7D9-4732-A63A-BED676ED34FB}");
                public static readonly ID SubtitleDiscountCreditLabel = new ID("{AD4808A1-FEAB-4F41-9B6C-9708462FF177}");
                public static readonly ID PromotionCodeLabel = new ID("{107FB405-7E12-4307-8A8A-DC8EFB3F1D20}");
                public static readonly ID AddPromoCodeLabel = new ID("{F995FFDA-5BCE-445B-AF7A-87F97865C88C}");
                public static readonly ID ApplyLoyaltyCreditLabel = new ID("{A00876BC-0935-4E34-B20B-C62A73770AF8}");
                public static readonly ID SubtitleLoyaltyCreditLabel = new ID("{A0135993-71DE-4545-AAE2-689FE6D74654}");
                public static readonly ID AddDiscountCardLabel = new ID("{5B9BB914-D42C-4791-8005-71AB64FFC53F}");

                public static readonly ID ShippingAddressLabel = new ID("{6DDE7489-4439-4483-B8DF-3BA3B44E74DC}");
                public static readonly ID EditShippingAddressLabel = new ID("{8C738971-A3E2-4AA2-BA36-5174E2143573}");
                public static readonly ID BillingAddressLabel = new ID("{BBE6905D-C685-4BCA-8E1B-E8A3D84710A0}");
                public static readonly ID EditBillingAddressLabel = new ID("{E8277201-5A95-464C-957C-CCC50FF74857}");
                public static readonly ID ShippingMethodLabel = new ID("{0624672E-1868-4954-8E34-34B22738FE71}");
                public static readonly ID EditShippingMethodLabel = new ID("{77E7262F-AF31-45BA-926C-19C7D2E6643E}");
                public static readonly ID DiscountCodeLabel = new ID("{B4C81E3B-DDBB-4177-92A6-A203AB4F11D9}");
                public static readonly ID EditDiscountCodeLabel = new ID("{6532C046-9650-41A8-B93F-67C79251E8C3}");
                public static readonly ID GiftCardsLabel = new ID("{013B350A-E2D8-497E-979B-7A19BD667944}");
                public static readonly ID EditGiftCardsLabel = new ID("{6FF1121A-9C79-4103-BB4A-F76F85BE739A}");
                public static readonly ID PaymentMethodsLabel = new ID("{3F42AD47-5396-4365-BBB9-61AABC99986B}");
                public static readonly ID EditPaymentMethodsLabe = new ID("{47F37A01-789A-45D9-B40B-AF82B289EE13}");
                public static readonly ID CreditCardLabel = new ID("{EB52264D-68A7-4760-AD38-85389723A27E}");
                public static readonly ID LoyaltyCardLabel = new ID("{A735E911-E581-438B-817E-0C289EE91335}");

                public static readonly ID DiscountLabel = new ID("{D8619DC5-A4C3-4E1B-A535-B5FC308E74EB}");
                public static readonly ID ColorLabel = new ID("{6029191F-8851-496B-86E7-4BFF30CFEE46}");
                public static readonly ID DeliveryLabel = new ID("{239A6669-1815-4BD9-960F-056553C521DE}");
            }
        }

        public struct OrderConfirmationCheckoutStep
        {
            public static readonly ID ID = new ID("{5F54B137-9CB1-4033-93E3-1201A01140DA}");

            public struct Fields
            {
                public static readonly ID EmailNotificationMessageLabel = new ID("{46B745C7-1B8C-4AD0-8BF6-56AF515A0969}");
                public static readonly ID OrderInformationLabel = new ID("{AFDF60C0-2044-4DD2-9965-585F4F1B0FD7}");
                public static readonly ID ConfirmationNumberLabel = new ID("{379F5A0F-9F68-4BE0-9856-C9400B22E420}");
                public static readonly ID StatusLabel = new ID("{E10771F2-2575-4919-9017-BD29945B8FFD}");
                public static readonly ID EmailSentMessage = new ID("{C24D9EBB-F805-42B6-BF99-835C1AC5FE71}");
                public static readonly ID PrintReceiptLabel = new ID("{1C5DCB0B-747F-48F0-B49B-A9B1AB4DA500}");
                public static readonly ID UpdatePreferencesLabel = new ID("{44A7A778-3E6E-448B-A8A1-AAD93A43BE8E}");
            }
        }
    }
}