namespace AMS.Services
{
    public interface ITicketGenerator
    {
        void AddTicket(int typeId, string summary, int tenantId);
    }
}