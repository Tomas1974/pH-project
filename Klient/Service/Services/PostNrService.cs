using infrastructure;
using infrastructure.Repositories;

namespace Service.Services;

public class PostNrService
{
    private readonly PostNrRespository _postNrRespository;

    public PostNrService(PostNrRespository postNrRespository)
    {
        _postNrRespository = postNrRespository;
    }


    public string findTown(int postNr)
    {
        return _postNrRespository.FindTown(postNr);
    }
}