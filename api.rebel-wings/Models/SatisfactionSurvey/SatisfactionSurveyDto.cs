namespace api.rebel_wings.Models.SatisfactionSurvey;

public class SatisfactionSurveyDto
{
    public SatisfactionSurveyDto()
    {
        PhotoSatisfactionSurveys = new HashSet<PhotoSatisfactionSurveyDto>();
    }
    public int Id { get; set; }
    public int BranchId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool QuestionOne { get; set; }
    public bool QuestionTwo { get; set; }
    public bool QuestionThree { get; set; }
    public bool QuestionFour { get; set; }
    public bool QuestionFive { get; set; }
    public bool QuestionSix { get; set; }
    public int Review { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public virtual ICollection<PhotoSatisfactionSurveyDto> PhotoSatisfactionSurveys { get; set; }
}
