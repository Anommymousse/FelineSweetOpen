using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DarkTonic.MasterAudio;
using DarkTonic.PoolBoss;
using DG.Tweening;
using Spine.Unity;
//using TreeEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using static MyExtensions.MyExtensions;

    public enum OrderPref
    {
        Right,
        Left,
        Up,
        Down
    }

public enum MummyMovement
{
    Right,
    Left,
    Up,
    Down,
    DownBreak,
    RightBreak,
    LeftBreak,
    UpBreak,
    Jump,
    Idle
}

    public class EnemyMummy : MonoBehaviour
    {
        Vector3 _OGposition;
        Vector3 _OGscale;
        Quaternion _OGrotation;
        bool _setupComplete=false;
        bool _setupUnderway; //Race condition :(
        bool _mummyIsMidBreakingBlock;
        Rigidbody2D _rigidbody2D;
        public float walkSpeed = 150.0f;
        public Tile tile;

        //Stop the character from rapid ping pong 
        bool _2ndPreferrence;
        float _2ndPreferrenceLastUse;
        float _2ndPreferranceDelay = 3.25f;

        Transform _playerGameObject;
        Transform _upBlockFeeler;
        Transform _leftBlockFeeler;
        Transform _rightBlockFeeler;
        Transform _downBlockFeeler;

        Transform _leftGround;
        Transform _rightGround;
        Transform _midGround;
        
        Transform _mummyArtTransform;
        SkeletonAnimation _mummyAnimation;

        BrickMap _brickMap;
        
        string[] _currentTileNames = new string[4];

        MummyMovement _mummyState;

        float _brickUnitDistance;
        
        Vector3 GetPlayerPosition()
        {
            return Player.GetWorldLocation();
        }

        void SetupMoveState()
        {
            if (_mummyState == MummyMovement.Jump) return;
            if (_mummyIsMidBreakingBlock) return;
            
            var playerPosition = GetPlayerPosition();
            var transpos = transform.position;
            var xdist = playerPosition.x - transpos.x;
            var ydist = playerPosition.y - transpos.y;
        
            var cellPosition = _brickMap.NonHiddenTilemap.WorldToCell(transpos);
            //Set to blank
            for(int i=0;i<3;i++)
                _currentTileNames[i] = "Blank";
            
            cellPosition.x += 1;
            var tile = _brickMap.NonHiddenTilemap.GetTile<Tile>(cellPosition);
            if (tile)
                _currentTileNames[0] = _brickMap.NonHiddenTilemap.GetTile<Tile>(cellPosition).name;
            
            cellPosition.x -= 2;
            tile = _brickMap.NonHiddenTilemap.GetTile<Tile>(cellPosition);
            if (tile)
                _currentTileNames[1] = _brickMap.NonHiddenTilemap.GetTile<Tile>(cellPosition).name;
            cellPosition.x += 1;

            
            cellPosition.y += 1;
            tile = _brickMap.NonHiddenTilemap.GetTile<Tile>(cellPosition);
            if (tile)
                _currentTileNames[2] = _brickMap.NonHiddenTilemap.GetTile<Tile>(cellPosition).name;
            cellPosition.y -= 2;
            tile = _brickMap.NonHiddenTilemap.GetTile<Tile>(cellPosition);
            if (tile)
                _currentTileNames[3] = _brickMap.NonHiddenTilemap.GetTile<Tile>(cellPosition).name;
            cellPosition.y += 1;
        
            //AI
            //If the way is blocked by solid then height match
            //Otherwise travel towards player destroying bricks and jumping straight up and creation of blocks
            //if the way of left/right is blocked 
        
            //What's our preferred direction
            OrderPref[] preferredMove = new OrderPref[2];
            if (Mathf.Abs(xdist) > Mathf.Abs(ydist))
            {
                preferredMove[0] = xdist > 0 ? OrderPref.Right : OrderPref.Left;
                preferredMove[1] = ydist > 0 ? OrderPref.Up : OrderPref.Down;
            }
            else
            {
                preferredMove[0] = ydist > 0 ? OrderPref.Up : OrderPref.Down;
                preferredMove[1] = xdist > 0 ? OrderPref.Right : OrderPref.Left;
            }
            
            //So we have the order but not whether the way is blocked by a solid
            if (IsWayBlocked(preferredMove[0]))
            {
                if (IsWayBlocked(preferredMove[1]))
                {
                    _mummyState = MummyMovement.Idle;
                    //Both blocked.
                    /*if(IsWayBlocked( OrderPref.Down ))
                    {
                        _mummyState = MummyMovement.Idle;
                    }
                    else
                    {
                        _mummyState = MummyMovement.Down;
                    }*/
                }
                else
                {
                    //Stop rapid ping pong
                    if (_2ndPreferrence )
                    {
                        if (Time.time > (_2ndPreferrenceLastUse + _2ndPreferranceDelay))
                        {
                            _2ndPreferrenceLastUse = Time.time;
                            SetMummyDirection(preferredMove[1]);    
                        }
                    }
                    else
                    {
                        _2ndPreferrence = true;
                        _2ndPreferrenceLastUse = Time.time;
                        //Go alt[1] way
                        SetMummyDirection(preferredMove[1]);
                    }
                }
            }
            else
            {
                _2ndPreferrence = false;
                //Go this way.[0]
                SetMummyDirection(preferredMove[0]);
            }
            
            //Log($" mummystate = {_mummyState} {Time.time} ");

            if(HasWayGotBrick(_mummyState))
            {
                DestroyBrickInDirection(_mummyState);
            }
        }

        void SetMummyDirection(OrderPref orderPref)
        {
            _mummyState = orderPref switch
            {
                OrderPref.Left => _mummyState = MummyMovement.Left,
                OrderPref.Right => _mummyState = MummyMovement.Right,
                OrderPref.Down => _mummyState = MummyMovement.Down,
                OrderPref.Up => _mummyState = MummyMovement.Up,
                _ => _mummyState = MummyMovement.Idle
            };
        }

        void SetMummyState(MummyMovement NewState)
        {
            _mummyState = NewState;
        }

        void DestroyBrickInDirection(MummyMovement mumState)
        {
            switch (mumState)
            {
                case MummyMovement.Right:
                    SetMummyState(MummyMovement.RightBreak);
                    
                    break;
                case MummyMovement.Left:
                    SetMummyState(MummyMovement.LeftBreak);
                    
                    break;
                case MummyMovement.Up:
                    SetMummyState(MummyMovement.UpBreak);
                    
                    break;
                case MummyMovement.Down:
                    SetMummyState(MummyMovement.DownBreak);
                    
                    break;
            }
        }

        bool HasWayGotBrick(MummyMovement mumState)
        {
            Log($" Details1 {_currentTileNames[0]} {_currentTileNames[1]} {_currentTileNames[2]} "); 
            Log($"[{_currentTileNames[3]}] at time{Time.time}");
            if (_currentTileNames[3] == null) _currentTileNames[3] = "Blank"; 
            if (_currentTileNames[3] == "") _currentTileNames[3] = "Blank"; 
            
            switch (mumState switch
            {
                MummyMovement.Right when _currentTileNames[0].ToLower().Contains("desert") => true,
                MummyMovement.Left when _currentTileNames[1].ToLower().Contains("desert") => true,
                MummyMovement.Up when _currentTileNames[2].ToLower().Contains("desert") => true,
                MummyMovement.Down when _currentTileNames[3].ToLower().Contains("desert") => true,
                _ => false
            })
            {
                case true:
                    return true;
                default:
                    return false;
            }
        }

        bool IsWayBlocked(OrderPref p1)
        {
            string cellName = p1 switch
            {
                OrderPref.Left => _currentTileNames[1],
                OrderPref.Right => _currentTileNames[0],
                OrderPref.Down => _currentTileNames[3],
                OrderPref.Up => _currentTileNames[2],
                _ => ""
            };
            
            if (cellName == null) return false;

            if (cellName.ToLower().Contains("static"))
            {
                return true;
            }
            
            return false;
        }


        //Note-: Race condition with spawner, needs to be delayed.
        void OnEnable()
        {
            //Log($"<color=red> On Enable for mummy position = {gameObject.transform.localPosition}</color>");
            
            //This is the start function due to poolboss.spawn
            if (_setupComplete == false)
                StartCoroutine(SetupWithWait());
            
            _mummyState = MummyMovement.Idle;
            
            //Race condition handler
            if (_setupUnderway) return;
            
            //Log($"<color=cyan> Sanity check {_OGposition} </color>");
            gameObject.transform.localPosition = _OGposition;
            gameObject.transform.localScale = _OGscale;
            gameObject.transform.rotation = _OGrotation;
            //Log($"<color=red> End of OnEnable for mummy position = {gameObject.transform.localPosition}</color>");
        }

        IEnumerator SetupWithWait()
        {
            _setupUnderway = true;
            yield return new WaitForSeconds(1.0f);
            _setupComplete = true;
            _mummyArtTransform = gameObject.transform.Find("MummyArt");
            _mummyAnimation = _mummyArtTransform.GetComponent<SkeletonAnimation>();
                
            _OGposition = gameObject.transform.localPosition;
            _OGrotation = gameObject.transform.localRotation;
            _OGscale = gameObject.transform.localScale;
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            _brickMap = GameObject.FindWithTag("BrickMap").GetComponent<BrickMap>();
            _brickUnitDistance = _brickMap.NonHiddenTilemap.CellToWorld(Vector3Int.one).y - _brickMap.NonHiddenTilemap.CellToWorld(Vector3Int.zero).y;

            gameObject.transform.localPosition = _OGposition;
            gameObject.transform.localScale = _OGscale;
            gameObject.transform.rotation = _OGrotation;
            
            _leftGround = gameObject.transform.Find("GroundLeft");
            _rightGround = gameObject.transform.Find("GroundRight");
            _midGround = gameObject.transform.Find("GroundMid");
            
            for(int i=0;i<3;i++)
                _currentTileNames[i] = "Blank";

        }

        void HandleMoveState()
        {
            Vector2 rb_speed;
            switch(_mummyState)
            {
                case MummyMovement.Right:
                    rb_speed = _rigidbody2D.velocity;
                    rb_speed.x = IsOnGroundThisFrame() ? walkSpeed * Time.deltaTime : 0f;
                    _rigidbody2D.velocity = rb_speed;
                    SetMummyFacingRight();
                    
                    break;
                case MummyMovement.Left:
                    rb_speed = _rigidbody2D.velocity;
                    rb_speed.x = IsOnGroundThisFrame() ? -walkSpeed * Time.deltaTime : 0f;
                    _rigidbody2D.velocity = rb_speed;
                    SetMummyFacingLeft();
                    break;
                case MummyMovement.Up:
                    _mummyState = MummyMovement.Jump;
                    StartCoroutine(MummyJumpAndPlaceBrick());
                    break;
                case MummyMovement.Down:
                    break;
                case MummyMovement.DownBreak:
                    DestroyBrickBelow();
                    break;
                case MummyMovement.RightBreak:
                    BreakBrickLR(MummyMovement.RightBreak);
                    break;
                case MummyMovement.LeftBreak:
                    BreakBrickLR(MummyMovement.LeftBreak);       
                    break;
                case MummyMovement.UpBreak:
                    _mummyState = MummyMovement.Jump;
                    StartCoroutine(MummyJumpAndPlaceBrick());
                    break;
                case MummyMovement.Idle:
                    SetMummyfacingTowardsPlayer();
                    break;
            }
        }

        void BreakBrickLR(MummyMovement direction)
        {
            int cellAdjust = 1;
            if (direction == MummyMovement.LeftBreak)
                cellAdjust = -1;
            if (direction == MummyMovement.RightBreak)
                cellAdjust = 1;

            var cellPosition = _brickMap.NonHiddenTilemap.WorldToCell(transform.position);
            cellPosition.x += cellAdjust;
            
            var tileToDestroy = _brickMap.NonHiddenTilemap.GetTile<Tile>(cellPosition);
            if (tileToDestroy)
            {
                VFX_DestroyBrickAt(cellPosition);        
                _brickMap.NonHiddenTilemap.SetTile(cellPosition,null);
            }
        }

        bool IsOnGroundThisFrame()
        {
            var collisionLayermask = 1<< LayerMask.NameToLayer("Bricks");
            var rchit = Physics2D.RaycastAll(_midGround.position, Vector2.down, 0.05f, collisionLayermask);
            var rchit1 = Physics2D.RaycastAll(_leftGround.position, Vector2.down, 0.05f, collisionLayermask);
            var rchit2 = Physics2D.RaycastAll(_rightGround.position, Vector2.down, 0.05f, collisionLayermask);
            
            if ((rchit.Length>=1)||(rchit1.Length>=1)||(rchit2.Length>=1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void SetMummyfacingTowardsPlayer()
        {
            var playerPosition = GetPlayerPosition();
            var transpos = transform.position;
            if (playerPosition.x > transpos.x)
            {
                SetMummyFacingRight();
            }
            else
            {
                SetMummyFacingLeft();
            }
        }

        void DestroyBrickBelow()
        {
            var brickCellPosition = _brickMap.NonHiddenTilemap.WorldToCell(transform.position);
            brickCellPosition.y -= 1;
            var direction = Vector2.down*0.75f;
            _brickMap.DestroyBrick(brickCellPosition);
            _mummyState = MummyMovement.Idle;
        }

        void SetMummyFacingRight()
        {
            var temp = _mummyArtTransform.localScale;
            if (temp.x < 0) temp.x *= -1;
            _mummyArtTransform.localScale = temp;
        }

        void SetMummyFacingLeft()
        {
            var temp = _mummyArtTransform.localScale;
            if (temp.x > 0) temp.x *= -1;
            _mummyArtTransform.localScale = temp;
        }

        
        
        IEnumerator MummyJumpAndPlaceBrick()
        {
            if (IsMidGroundOnGroundThisFrame() == false)
            {
                _mummyState = MummyMovement.Idle;
                yield break;
            }
            
            var currentYPosition = transform.position.y;
            var endJumpPosition = currentYPosition + _brickUnitDistance;
            var brickCellPosition = _brickMap.NonHiddenTilemap.WorldToCell(transform.position);
            
            //Check if destructible brick above
            
            brickCellPosition.y += 1;

            //?????
            if (IsWayBlocked(OrderPref.Up))
            {
                _mummyState = MummyMovement.Idle;
                yield break;
            }
                
            var ceilingbrick = brickCellPosition;
            var ceilingtile = _brickMap.NonHiddenTilemap.GetTile<Tile>(ceilingbrick);
            if (ceilingtile)
            {
                VFX_DestroyBrickAt(ceilingbrick);        
                _brickMap.NonHiddenTilemap.SetTile(ceilingbrick,null);
            }
            
            brickCellPosition.y -= 1; //For brick in current player position.
            var belowBrick = brickCellPosition;
            
            //Jump
            float jumpTimer = 1.0f;
            float jumpSpeed = (endJumpPosition-currentYPosition)/jumpTimer;
            float startTime = Time.time;
            float endTime = startTime + jumpTimer;

            var currentLocalPosition = transform.localPosition;
            while (Time.time < endTime)
            {
                currentYPosition += jumpSpeed*Time.deltaTime;
                currentLocalPosition.y = currentYPosition;
                transform.localPosition = currentLocalPosition;
                yield return null;
            }
            
            //Place Brick
            _brickMap.NonHiddenTilemap.SetTile(belowBrick,tile);
            MasterAudio.PlaySound("Stone 5");

            _mummyState = MummyMovement.Idle;
        }

        void VFX_DestroyBrickAt(Vector3Int ceilingbrick)
        {
            PoolBoss.SpawnInPool("EnemyDestroyBrick",_brickMap.NonHiddenTilemap.CellToWorld(ceilingbrick),Quaternion.identity);
        }

        // Update is called once per frame
        void Update()
        {
            if (_setupComplete is false) return;
            
            //Log($"<color=red> Update for mummy position st1 = {gameObject.transform.position}</color>");
            SetupMoveState();
            //Log($"<color=red> Update for mummy position st1b = {gameObject.transform.position}</color>");
            HandleMoveState();
            //Log($"<color=red> Update for mummy position st2 = {gameObject.transform.position}</color>");
            StopFallingIfOnGround();
        }

        void StopFallingIfOnGround()
        {
            var collisionLayermask = 1<< LayerMask.NameToLayer("Bricks");
            var rchit = Physics2D.RaycastAll(_midGround.position, Vector2.down, 0.05f, collisionLayermask);
            var rchit1 = Physics2D.RaycastAll(_leftGround.position, Vector2.down, 0.05f, collisionLayermask);
            var rchit2 = Physics2D.RaycastAll(_rightGround.position, Vector2.down, 0.05f, collisionLayermask);

            if ((rchit.Length <= 0) && (rchit1.Length <= 0) && (rchit2.Length <= 0)) return;

            var groundPresent = (rchit.Length >= 1)||(rchit1.Length >= 1)||(rchit2.Length >= 1);

            if ((groundPresent is false) || (_mummyState == MummyMovement.Jump)) return;
            
            var temp = _rigidbody2D.velocity;
            temp.y = 0;
            _rigidbody2D.velocity = temp;
        }

        bool IsMidGroundOnGroundThisFrame()
        {
            var collisionLayermask = 1<< LayerMask.NameToLayer("Bricks");
            var rchit = Physics2D.RaycastAll(_midGround.position, Vector2.down, 0.05f, collisionLayermask);
            if ((rchit.Length >= 1))
            {
                return true;
            }
            return false;
        }
        

    }