namespace WebUI.Client.Dtos;

public record UploadResultDto(bool Uploaded, string FileName, string StoredFileName, int ErrorCode);
