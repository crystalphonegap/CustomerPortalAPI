using CustomerPortalWebApi.Models;
using System.Collections.Generic;

namespace CustomerPortalWebApi.Interface
{
    public interface IEscalationService
    {
        long InsertEscalationMatrix(EscalationMatrixModel model);

        long UpdateEscalationMatrix(EscalationMatrixModel model);

        List<EscalationMatrixModel> GetEscalationMatrix();

        EscalationMatrixModel GetEscalationMatrixByID(long id);
    }
}