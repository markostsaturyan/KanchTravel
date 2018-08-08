namespace Authentication.DataManagement.DataAccesLayer.DataAccessLayerDataModel
{
    public class UserIdentifiers
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public string UserGuid { get; set; }
        public bool IsApproved { get; set; }
    }
}
