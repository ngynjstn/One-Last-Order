namespace DefaultNamespace
{
    public interface IInteractable
    {
        public string InteractableHintText { get; }
        public bool IsInteractable { get; }
        public void Interact();
    }
}