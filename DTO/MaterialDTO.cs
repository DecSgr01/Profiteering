namespace Profiteering.DTO;
internal class MaterialDTO
{
    public int id { get; set; }
    public string name { get; set; }
    public float count { get; set; }
    public float averageUnitPrice { get; set; }

    public MaterialDTO(int id, string name, float count)
    {
        this.id = id;
        this.name = name;
        this.count = count;
    }

    public override string ToString()
    {
        return "{id:" + id + ",name:" + name + ",count:" + count + "}";
    }
}