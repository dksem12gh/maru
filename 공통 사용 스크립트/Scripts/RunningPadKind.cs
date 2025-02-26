//각 게임별 버튼 갯수에 따른 버튼 상태 구분    
namespace DigitalMaru
{
    //ex) MID3BTN = MID[버튼 특징] 3[버튼갯수] BTN[버튼/미니게임발판] - 버튼 특징은 따로 정리된게 없어서 기획서 참고 비슷한 버튼 끼리 묶자...

    public enum MID3BTN
    {
        P0_0,
        P0_1,
        P0_M,
        P1_0,
        P1_1,
        P1_M,
    }
    
    public enum JUMP2BTN
    {
        P0_0,
        P0_1,
        P1_0,
        P1_1,
    }

    public enum JUMP3BTN
    {
        P0_0,
        P0_1,
        P0_2,
        P1_0,
        P1_1,
        P1_2,
    }

    public enum SPLITBTN
    {

    }
}