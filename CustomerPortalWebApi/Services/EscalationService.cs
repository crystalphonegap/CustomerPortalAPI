using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;

namespace CustomerPortalWebApi.Services
{
    public class EscalationService : IEscalationService
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;

        public EscalationService(ICustomerPortalHelper customerPortalHelper)
        {
            _customerPortalHelper = customerPortalHelper;
        }

        public long InsertEscalationMatrix(EscalationMatrixModel model)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("IDbint", 0, DbType.Int64);
            dbPara.Add("CategoryNamevtxt", model.CategoryNamevtxt, DbType.String);
            dbPara.Add("AssignTovtxt", model.AssignTovtxt, DbType.String);
            dbPara.Add("EscalationDays1int", model.EscalationDays1int, DbType.Int32);
            dbPara.Add("Escalation1AssignTovtxt", model.Escalation1AssignTovtxt, DbType.String);
            dbPara.Add("EscalationDays2int", model.EscalationDays2int, DbType.Int32);
            dbPara.Add("Escalation2AssignTovtxt", model.Escalation2AssignTovtxt, DbType.String);
            dbPara.Add("EscalationDays3int", model.EscalationDays3int, DbType.Int32);
            dbPara.Add("Escalation3AssignTovtxt", model.Escalation3AssignTovtxt, DbType.String);
            dbPara.Add("CreatedByvtxt", model.CreatedByvtxt, DbType.String);
            dbPara.Add("Mode", "A", DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertUpdateEscalationMatrix]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public long UpdateEscalationMatrix(EscalationMatrixModel model)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("IDbint", model.IDbint, DbType.Int64);
            dbPara.Add("CategoryNamevtxt", model.CategoryNamevtxt, DbType.String);
            dbPara.Add("AssignTovtxt", model.AssignTovtxt, DbType.String);
            dbPara.Add("EscalationDays1int", model.EscalationDays1int, DbType.Int32);
            dbPara.Add("Escalation1AssignTovtxt", model.Escalation1AssignTovtxt, DbType.String);
            dbPara.Add("EscalationDays2int", model.EscalationDays2int, DbType.Int32);
            dbPara.Add("Escalation2AssignTovtxt", model.Escalation2AssignTovtxt, DbType.String);
            dbPara.Add("EscalationDays3int", model.EscalationDays3int, DbType.Int32);
            dbPara.Add("Escalation3AssignTovtxt", model.Escalation3AssignTovtxt, DbType.String);
            dbPara.Add("CreatedByvtxt", model.CreatedByvtxt, DbType.String);
            dbPara.Add("Mode", "M", DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertUpdateEscalationMatrix]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public List<EscalationMatrixModel> GetEscalationMatrix()
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("IDbint", 0, DbType.Int64);
            var data = _customerPortalHelper.GetAll<EscalationMatrixModel>("[dbo].[uspviewEscalationMatrix]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;
        }

        public EscalationMatrixModel GetEscalationMatrixByID(long id)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("IDbint", id, DbType.Int64);
            var data = _customerPortalHelper.GetAll<EscalationMatrixModel>("[dbo].[uspviewEscalationMatrix]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
          
            if (data != null)
            {
                return   data[0];
            }
            else
            {
                return null;
            }
        }
    }
}