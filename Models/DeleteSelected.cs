namespace asp.Models;

public class DeleteSelected
{
    public record DeleteSelectedRequest(long[]? Ids, string? Category);

}