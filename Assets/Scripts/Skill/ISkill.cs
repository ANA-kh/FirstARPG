namespace FirstARPG.Skill
{
    public interface ISkillBase
    { 
        int ID { get; }
        void SerializeRecovery(SkillContext skillContext);
    }

    public interface ISkill : ISkillBase
    {
        void OnTemplateRegister();
        void OnTemplateUnRegister();
        ISkillBase Instantiate(SkillContext skillContext);
    }
}