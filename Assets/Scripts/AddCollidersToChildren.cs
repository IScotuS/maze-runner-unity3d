using UnityEngine;

public class AddCollidersToChildren : MonoBehaviour
{
    public enum ColliderType
    {
        Box,
        Sphere,
        Capsule,
        Mesh,
    }

    public ColliderType colliderType = ColliderType.Box;

    // Customizable properties for the colliders
    public Vector3 boxColliderSize = new Vector3(1f, 1f, 1f);
    public float sphereColliderRadius = 0.5f;
    public float capsuleColliderRadius = 0.5f;
    public float capsuleColliderHeight = 2f;

    void Start()
    {
        foreach (Transform child in transform)
        {
            AddColliderToChild(child);
        }
    }

    void AddColliderToChild(Transform child)
    {
        Collider colliderComponent = null;

        switch (colliderType)
        {
            case ColliderType.Box:
                BoxCollider boxCollider = child.gameObject.AddComponent<BoxCollider>();
                boxCollider.size = boxColliderSize;
                colliderComponent = boxCollider;
                break;
            case ColliderType.Sphere:
                SphereCollider sphereCollider = child.gameObject.AddComponent<SphereCollider>();
                sphereCollider.radius = sphereColliderRadius;
                colliderComponent = sphereCollider;
                break;
            case ColliderType.Capsule:
                CapsuleCollider capsuleCollider = child.gameObject.AddComponent<CapsuleCollider>();
                capsuleCollider.radius = capsuleColliderRadius;
                capsuleCollider.height = capsuleColliderHeight;
                colliderComponent = capsuleCollider;
                break;
            case ColliderType.Mesh:
                colliderComponent = child.gameObject.AddComponent<MeshCollider>();
                break;
        }

        // You can set more collider properties here if needed
    }
}
