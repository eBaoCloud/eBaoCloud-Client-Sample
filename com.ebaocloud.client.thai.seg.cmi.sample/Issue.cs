﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// using ebaoCloud cmi client namespace
using com.ebaocloud.client.thai.seg.cmi.api;
using com.ebaocloud.client.thai.seg.cmi.parameters;
using com.ebaocloud.client.thai.seg.cmi.response;

namespace com.ebao.gs.ebaocloud.sea.seg.cmi.sample
{
    public class Issue
    {
        public String IssueAction()
        {
            PolicyService service = new PolicyServiceImpl();
            LoginResp resp = service.Login(Login.sampleUserName, Login.samplePassword);

            Policy policyParam = new Policy();
            List<Document> documents = new List<Document>();
            Document doc = new Document();
            doc.documentType = "2";
            doc.name = "test";
            doc.file = new System.IO.FileInfo("../../UploadSample.txt");
            documents.Add(doc);
            policyParam.documents = documents;

            policyParam.effectiveDate = DateTime.Now.ToLocalTime();
            policyParam.expireDate = DateTime.Now.AddYears(1).ToLocalTime();
            policyParam.proposalDate = DateTime.Now.ToLocalTime();
            policyParam.productCode = "CMI";
            policyParam.productVersion = "v1";
            policyParam.isPayerSameAsPolicyholder = true;

            String randomStr = new Random(DateTime.Now.Millisecond).Next().ToString();
            policyParam.insured = new Insured();
            policyParam.insured.vehicleChassisNo = "CN" + randomStr;
            policyParam.insured.vehicleRegistrationNo = "CN" + randomStr;
            policyParam.insured.vehicleColor = "white";
            policyParam.insured.vehicleCountry = "THA";
            policyParam.insured.vehicleModelDescription = "Sedan 4dr G  6sp FWD 2.5 2016";
            policyParam.insured.vehicleMakeName = "TOYOTA";
            policyParam.insured.vehicleProvince = "THA";
            policyParam.insured.vehicleRegistrationYear = 2016;
            policyParam.insured.vehicleModelYear = 2016;

            MasterDataService masterDataService = new MasterDataServiceImpl();
            List<KeyValue> vehicleTypes = masterDataService.GetVehicleType();
            List<CascadeValue> vehicleSubTypes = masterDataService.GetVehicleSubType(vehicleTypes[0].key);
            List<KeyValue> vehicleUsages = masterDataService.GetVehicleUsage(vehicleSubTypes[0].key);

            policyParam.insured.vehicleType = vehicleTypes[0].key;
            policyParam.insured.vehicleSubType = vehicleSubTypes[0].key;
            policyParam.insured.vehicleUsage = vehicleUsages[0].key;

            policyParam.payer = new Payer();
            policyParam.payer.inThaiAddress = new InThaiAddress();
            policyParam.payer.inThaiAddress.smartlyMatchAddress = true;
            policyParam.payer.inThaiAddress.fullAddress = "ชั้น 24 อาคารสาธรซิตี้ทาวเวอร์ 175 ทุ่งมหาเมฆ สาทร กรุงเทพฯ 10120";
            policyParam.payer.name = "Jacky Cheng";

            policyParam.indiPolicyholder = new IndividualPolicyholder();
            policyParam.indiPolicyholder.idNo = "123456";
            policyParam.indiPolicyholder.idType = "1";
            policyParam.indiPolicyholder.inThaiAddress = new InThaiAddress();
            policyParam.indiPolicyholder.inThaiAddress.smartlyMatchAddress = true;
            policyParam.indiPolicyholder.inThaiAddress.fullAddress = "24 (318 เดิม) ซ.อุดมสุข30 แยก2 ถ.อุดมสุข แขวงบางนา เขตบางนา กทม. 10260";
            policyParam.indiPolicyholder.inThaiAddress.district = "1001";
            policyParam.indiPolicyholder.inThaiAddress.postalCode = "10200";
            policyParam.indiPolicyholder.inThaiAddress.province = "10";
            policyParam.indiPolicyholder.inThaiAddress.street = "songhu rd.";
            policyParam.indiPolicyholder.inThaiAddress.subDistrict = "100101";
            policyParam.indiPolicyholder.lastName = "Cheng";
            policyParam.indiPolicyholder.firstName = "Jacky";
            policyParam.indiPolicyholder.mobile = "1234999";
            policyParam.indiPolicyholder.taxNo = "10000";
            policyParam.indiPolicyholder.title = IndividualPrefix.Khun;

            IssuedResp issueResp = service.Issue(resp.token, policyParam);
             if (issueResp.success)
            {
                Console.WriteLine("Issued succcess: true" + "\nPolicyNo:" + issueResp.policyNo);
            } else
            {
                Console.WriteLine("Issued succcess: false" + "\nMessage:" + issueResp.message);
            }
            return issueResp.policyNo;
        }
    }
}
