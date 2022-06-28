using MetadataScrapper;
using Microsoft.AspNetCore.Mvc;

namespace MetadataAPI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class FacebookController : ControllerBase
{
    private readonly MetaFacebook _metaFacebook;
    private readonly BaseResponse _baseResponse;

    public FacebookController(MetaFacebook metaFacebook, BaseResponse baseResponse)
    {
        _metaFacebook = metaFacebook;
        _baseResponse = baseResponse;
    }

    public async Task<IActionResult> Likes(string url)
    {
        _baseResponse.Result = await _metaFacebook.PostLikes(url);
        return Ok(_baseResponse);
    }

    public async Task<IActionResult> Page(string url)
    {
        _baseResponse.Result = await _metaFacebook.Likes(url);
        return Ok(_baseResponse);
    }

    public async Task<IActionResult> Comments(string url)
    {
        _baseResponse.Result = await _metaFacebook.Comments(url);
        return Ok(_baseResponse);
    }

    public async Task<IActionResult> Shares(string url)
    {
        _baseResponse.Result = await _metaFacebook.Shares(url);
        return Ok(_baseResponse);
    }
}