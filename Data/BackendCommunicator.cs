namespace net.shonx.privatenotes.frontend.Data;

using System.Net.Http.Headers;
using net.shonx.privatenotes.backend;
using net.shonx.privatenotes.backend.requests;

public class BackendCommunicator
{
    public async Task<Note?> GetNote(string token, string SecretName)
    {
        if (string.IsNullOrEmpty(token))
            throw new NullReferenceException();
        if (string.IsNullOrEmpty(SecretName))
            return null;
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Program.BACKEND + "note/get/");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        GetNoteRequest GNR = new GetNoteRequest(SecretName);
        return await Helper.WriteRequestWithReply<Note?>(request, GNR);
    }

    public async Task<List<Note>?> GetAllNotes(string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new NullReferenceException();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Program.BACKEND + "note/getall/");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await Helper.WriteRequestWithReply<List<Note>>(request, null);
    }

    public async Task<AKVHResponse> CreateNote(string token, Note Note)
    {
        if (string.IsNullOrEmpty(token))
            throw new NullReferenceException();
        if (Note is null)
            return AKVHResponse.NULL_REQUEST;
        if (string.IsNullOrEmpty(Note.Name) || string.IsNullOrEmpty(Note.Value))
            return AKVHResponse.NULL_REQUEST;
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, Program.BACKEND + "note/create/");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        CreateNoteRequest CNR = new CreateNoteRequest(Note.Name, Note.Value);
        return await Helper.WriteRequest(request, CNR);
    }

    public async Task<AKVHResponse> DeleteNote(string token, string SecretName)
    {
        if (string.IsNullOrEmpty(token))
            throw new NullReferenceException();
        if (string.IsNullOrEmpty(SecretName))
            return AKVHResponse.NULL_REQUEST;
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, Program.BACKEND + "note/delete/");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        DeleteNoteRequest DNR = new(SecretName);
        return await Helper.WriteRequest(request, DNR);
    }

    public async Task<AKVHResponse> UpdateNote(string token, string SecretName, string? newName = null, string? newValue = null)
    {
        if (string.IsNullOrEmpty(token))
            throw new NullReferenceException();
        if (string.IsNullOrEmpty(SecretName))
            return AKVHResponse.NULL_REQUEST;
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Program.BACKEND + "note/update/");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        UpdateNoteRequest UDR = new(SecretName, newName, newValue);
        return await Helper.WriteRequest(request, UDR);
    }


}