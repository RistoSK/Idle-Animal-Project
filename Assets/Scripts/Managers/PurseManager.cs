using UnityEngine;

public class PurseManager : MonoBehaviour
{
    [SerializeField] private PurseHud _purseHud;
    [SerializeField] private PurseAttribute _purseAttribute;
    
    public static PurseManager Instance;

    public PurseAttribute GetPurseAttribute => _purseAttribute;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        AnimalManager.Instance.OnResourceCollected += ResourceCollected;
        Game.Instance.OnPurseUpdated += UpdatePurseHud;
        
        _purseHud.UpgradePurseHudValues(_purseAttribute.CurrentResources);
    }

    public bool TryToPurchase(int amountToBePaid)
    {
        if (amountToBePaid > _purseAttribute.CurrentResources)
        {
            return false;
        }
        
        _purseAttribute.CurrentResources -= amountToBePaid;
        _purseHud.UpgradePurseHudValues(_purseAttribute.CurrentResources);
        return true;
    }
    
    private void ResourceCollected(float resourceAmount)
    {
        _purseAttribute.CurrentResources += resourceAmount;
        _purseHud.UpgradePurseHudValues(_purseAttribute.CurrentResources);
    }

    private void UpdatePurseHud()
    {
        _purseHud.UpgradePurseHudValues(_purseAttribute.CurrentResources);
    }
}
