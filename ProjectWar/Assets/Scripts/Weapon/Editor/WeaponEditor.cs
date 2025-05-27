// Place this in an `Editor/` folder as WeaponEditor.cs
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{
    SerializedProperty weaponClassProp;

    // Player
    SerializedProperty playerCameraProp, playerBulletPrefabProp, playerBulletSpawnProp;
    // Tank
    SerializedProperty tankCameraProp, tankShellPrefabProp, tankShellSpawnProp ,
                       tankBodyProp, tankTurretProp, tankMoveSpeedProp, turretRotateSpeedProp,crosshairUIProp, 
                       crosshairMaxDistanceProp;
    // Melee
    SerializedProperty meleeKeyProp, meleeRangeProp, meleeDelayProp, meleeHitLayersProp, animatorProp;
    // Common
    SerializedProperty shootingModeProp, shootingDelayProp, bulletsPerBurstProp,
                       spreadIntensityProp, bulletVelocityProp, bulletPrefabLifeTimeProp;

    void OnEnable()
    {
        weaponClassProp            = serializedObject.FindProperty("weaponClass");

        playerCameraProp           = serializedObject.FindProperty("playerCamera");
        playerBulletPrefabProp     = serializedObject.FindProperty("playerBulletPrefab");
        playerBulletSpawnProp      = serializedObject.FindProperty("playerBulletSpawn");

        tankCameraProp             = serializedObject.FindProperty("tankCamera");
        tankShellPrefabProp        = serializedObject.FindProperty("tankShellPrefab");
        tankShellSpawnProp         = serializedObject.FindProperty("tankShellSpawn");
        tankBodyProp        = serializedObject.FindProperty("tankBody");
        tankTurretProp      = serializedObject.FindProperty("tankTurret");
        tankMoveSpeedProp   = serializedObject.FindProperty("tankMoveSpeed");
        turretRotateSpeedProp = serializedObject.FindProperty("turretRotateSpeed");
        crosshairUIProp = serializedObject.FindProperty("crosshairUI");
        crosshairMaxDistanceProp = serializedObject.FindProperty("crosshairMaxDistance");
        
        meleeKeyProp               = serializedObject.FindProperty("meleeKey");
        meleeRangeProp             = serializedObject.FindProperty("meleeRange");
        meleeDelayProp             = serializedObject.FindProperty("meleeDelay");
        meleeHitLayersProp         = serializedObject.FindProperty("meleeHitLayers");
        animatorProp               = serializedObject.FindProperty("animator");

        shootingModeProp           = serializedObject.FindProperty("currentShootingMode");
        shootingDelayProp          = serializedObject.FindProperty("shootingDelay");
        bulletsPerBurstProp        = serializedObject.FindProperty("bulletsPerBurst");
        spreadIntensityProp        = serializedObject.FindProperty("spreadIntensity");
        bulletVelocityProp         = serializedObject.FindProperty("bulletVelocity");
        bulletPrefabLifeTimeProp   = serializedObject.FindProperty("bulletPrefabLifeTime");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(weaponClassProp);

        switch ((Weapon.WeaponClass)weaponClassProp.enumValueIndex)
        {
            case Weapon.WeaponClass.Player:
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Player Weapon Settings", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(playerCameraProp);
                EditorGUILayout.PropertyField(playerBulletPrefabProp);
                EditorGUILayout.PropertyField(playerBulletSpawnProp);
                break;

            case Weapon.WeaponClass.Tank:
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Tank Weapon Settings", EditorStyles.boldLabel);
                // draw each only once
                EditorGUILayout.PropertyField(tankCameraProp);
                EditorGUILayout.PropertyField(tankShellPrefabProp);
                EditorGUILayout.PropertyField(tankShellSpawnProp);
                EditorGUILayout.PropertyField(crosshairUIProp);
                EditorGUILayout.PropertyField(crosshairMaxDistanceProp);
                
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Movement Settings", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(tankBodyProp);
                EditorGUILayout.PropertyField(tankTurretProp);
                EditorGUILayout.PropertyField(tankMoveSpeedProp);
                EditorGUILayout.PropertyField(turretRotateSpeedProp);
                break;


            case Weapon.WeaponClass.Melee:
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Melee Settings", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(meleeKeyProp);
                EditorGUILayout.PropertyField(meleeRangeProp);
                EditorGUILayout.PropertyField(meleeDelayProp);
                EditorGUILayout.PropertyField(meleeHitLayersProp);
                EditorGUILayout.PropertyField(animatorProp);
                break;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Common Shooting Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(shootingModeProp);
        EditorGUILayout.PropertyField(shootingDelayProp);
        EditorGUILayout.PropertyField(bulletsPerBurstProp);
        EditorGUILayout.PropertyField(spreadIntensityProp);
        EditorGUILayout.PropertyField(bulletVelocityProp);
        EditorGUILayout.PropertyField(bulletPrefabLifeTimeProp);

        serializedObject.ApplyModifiedProperties();
    }
}
