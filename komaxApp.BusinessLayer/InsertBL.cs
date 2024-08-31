using komaxApp.DatabaseLayer;
using KomaxApp.Model.Create;
using KomaxApp.Model.LoadTest;
using KomaxApp.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace komaxApp.BusinessLayer
{
    public class InsertBL
    {
        public void InsertComboPortsBL(string _cbPowerMeter, string _cbTorqueMeter, string _cbRPM, string _cbTemperature)
        {
            new InsertDL().InsertComboPortsDL(_cbPowerMeter, _cbTorqueMeter, _cbRPM, _cbTemperature);
        }
            public CreateModel.Response InsertRecordsBL(VmCreateMotor v, string IsEdit)
        {
            CreateModel.Response response = new CreateModel.Response();
            try
            {
                response = new InsertDL().InsertRecords(v, IsEdit);
            }
            catch (Exception ex)
            {
                throw new Exception("BL Error: " + ex.Message);
            }
            return response;
        }

        public RequestLoadTestModel.Response InsertRecordNoLoadPointBL(RequestLoadTestModel.Request v)
        {
            RequestLoadTestModel.Response response = new RequestLoadTestModel.Response();
            try
            {
                response = new InsertDL().InsertRecordNoLoadPointDL(v);
                if (response.StatusCode == 1)
                {
                    JIMessageBox.InformationMessage(response.StatusDetails);
                }
                else
                {
                    JIMessageBox.ErrorMessage(response.StatusDetails);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("BL Error: " + ex.Message);
            }
            return response;
        }
    }
}
