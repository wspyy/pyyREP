using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityKore.Web.AppCode
{
    public class DesKey
    {
        private static readonly uint[] Df_Key = { 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef, 0xfe, 0xdc, 0xba, 0x98, 0x76, 0x54, 0x32,
                0x10, 0x89, 0xab, 0xcd, 0xef, 0x01, 0x23, 0x45, 0x67 };

        private static readonly uint[] bytebit = { 128, 64, 32, 16, 8, 4, 2, 1 };

        private static readonly uint[] bigbyte = { 0x800000, 0x400000, 0x200000, 0x100000, 0x80000, 0x40000, 0x20000, 0x10000, 0x8000,
                0x4000, 0x2000, 0x1000, 0x800, 0x400, 0x200, 0x100, 0x80, 0x40, 0x20, 0x10, 0x8, 0x4, 0x2, 0x1 };

        private static readonly uint[] pc1 = { 56, 48, 40, 32, 24, 16, 8, 0, 57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10, 2,
                59, 51, 43, 35, 62, 54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 60, 52, 44, 36, 28, 20, 12,
                4, 27, 19, 11, 3 };

        private static readonly uint[] totrot = { 1, 2, 4, 6, 8, 10, 12, 14, 15, 17, 19, 21, 23, 25, 27, 28 };

        private static readonly uint[] pc2 ={ 13, 16, 10, 23, 0, 4, 2, 27, 14, 5, 20, 9, 22, 18, 11, 3, 25, 7, 15, 6, 26, 19, 12, 1, 40,
                51, 30, 36, 46, 54, 29, 39, 50, 44, 32, 47, 43, 48, 38, 55, 33, 52, 45, 41, 49, 35, 28, 31};

        private static readonly uint[] SP1 = { 0x01010400, 0x00000000, 0x00010000, 0x01010404, 0x01010004, 0x00010404, 0x00000004,
            0x00010000, 0x00000400, 0x01010400, 0x01010404, 0x00000400, 0x01000404, 0x01010004, 0x01000000, 0x00000004,
            0x00000404, 0x01000400, 0x01000400, 0x00010400, 0x00010400, 0x01010000, 0x01010000, 0x01000404, 0x00010004,
            0x01000004, 0x01000004, 0x00010004, 0x00000000, 0x00000404, 0x00010404, 0x01000000, 0x00010000, 0x01010404,
            0x00000004, 0x01010000, 0x01010400, 0x01000000, 0x01000000, 0x00000400, 0x01010004, 0x00010000, 0x00010400,
            0x01000004, 0x00000400, 0x00000004, 0x01000404, 0x00010404, 0x01010404, 0x00010004, 0x01010000, 0x01000404,
            0x01000004, 0x00000404, 0x00010404, 0x01010400, 0x00000404, 0x01000400, 0x01000400, 0x00000000, 0x00010004,
            0x00010400, 0x00000000, 0x01010004 };

        private static readonly uint[] SP2 = { 0x80108020, 0x80008000, 0x00008000, 0x00108020, 0x00100000, 0x00000020, 0x80100020,
                0x80008020, 0x80000020, 0x80108020, 0x80108000, 0x80000000, 0x80008000, 0x00100000, 0x00000020, 0x80100020,
                0x00108000, 0x00100020, 0x80008020, 0x00000000, 0x80000000, 0x00008000, 0x00108020, 0x80100000, 0x00100020,
                0x80000020, 0x00000000, 0x00108000, 0x00008020, 0x80108000, 0x80100000, 0x00008020, 0x00000000, 0x00108020,
                0x80100020, 0x00100000, 0x80008020, 0x80100000, 0x80108000, 0x00008000, 0x80100000, 0x80008000, 0x00000020,
                0x80108020, 0x00108020, 0x00000020, 0x00008000, 0x80000000, 0x00008020, 0x80108000, 0x00100000, 0x80000020,
                0x00100020, 0x80008020, 0x80000020, 0x00100020, 0x00108000, 0x00000000, 0x80008000, 0x00008020, 0x80000000,
                0x80100020, 0x80108020, 0x00108000 };

        private static readonly uint[] SP3 = { 0x00000208, 0x08020200, 0x00000000, 0x08020008, 0x08000200, 0x00000000, 0x00020208,
                0x08000200, 0x00020008, 0x08000008, 0x08000008, 0x00020000, 0x08020208, 0x00020008, 0x08020000, 0x00000208,
                0x08000000, 0x00000008, 0x08020200, 0x00000200, 0x00020200, 0x08020000, 0x08020008, 0x00020208, 0x08000208,
                0x00020200, 0x00020000, 0x08000208, 0x00000008, 0x08020208, 0x00000200, 0x08000000, 0x08020200, 0x08000000,
                0x00020008, 0x00000208, 0x00020000, 0x08020200, 0x08000200, 0x00000000, 0x00000200, 0x00020008, 0x08020208,
                0x08000200, 0x08000008, 0x00000200, 0x00000000, 0x08020008, 0x08000208, 0x00020000, 0x08000000, 0x08020208,
                0x00000008, 0x00020208, 0x00020200, 0x08000008, 0x08020000, 0x08000208, 0x00000208, 0x08020000, 0x00020208,
                0x00000008, 0x08020008, 0x00020200 };

        private static readonly uint[] SP4 = { 0x00802001, 0x00002081, 0x00002081, 0x00000080, 0x00802080, 0x00800081, 0x00800001,
                0x00002001, 0x00000000, 0x00802000, 0x00802000, 0x00802081, 0x00000081, 0x00000000, 0x00800080, 0x00800001,
                0x00000001, 0x00002000, 0x00800000, 0x00802001, 0x00000080, 0x00800000, 0x00002001, 0x00002080, 0x00800081,
                0x00000001, 0x00002080, 0x00800080, 0x00002000, 0x00802080, 0x00802081, 0x00000081, 0x00800080, 0x00800001,
                0x00802000, 0x00802081, 0x00000081, 0x00000000, 0x00000000, 0x00802000, 0x00002080, 0x00800080, 0x00800081,
                0x00000001, 0x00802001, 0x00002081, 0x00002081, 0x00000080, 0x00802081, 0x00000081, 0x00000001, 0x00002000,
                0x00800001, 0x00002001, 0x00802080, 0x00800081, 0x00002001, 0x00002080, 0x00800000, 0x00802001, 0x00000080,
                0x00800000, 0x00002000, 0x00802080 };

        private static readonly uint[] SP5 = { 0x00000100, 0x02080100, 0x02080000, 0x42000100, 0x00080000, 0x00000100, 0x40000000,
                0x02080000, 0x40080100, 0x00080000, 0x02000100, 0x40080100, 0x42000100, 0x42080000, 0x00080100, 0x40000000,
                0x02000000, 0x40080000, 0x40080000, 0x00000000, 0x40000100, 0x42080100, 0x42080100, 0x02000100, 0x42080000,
                0x40000100, 0x00000000, 0x42000000, 0x02080100, 0x02000000, 0x42000000, 0x00080100, 0x00080000, 0x42000100,
                0x00000100, 0x02000000, 0x40000000, 0x02080000, 0x42000100, 0x40080100, 0x02000100, 0x40000000, 0x42080000,
                0x02080100, 0x40080100, 0x00000100, 0x02000000, 0x42080000, 0x42080100, 0x00080100, 0x42000000, 0x42080100,
                0x02080000, 0x00000000, 0x40080000, 0x42000000, 0x00080100, 0x02000100, 0x40000100, 0x00080000, 0x00000000,
                0x40080000, 0x02080100, 0x40000100 };

        private static readonly uint[] SP6 = { 0x20000010, 0x20400000, 0x00004000, 0x20404010, 0x20400000, 0x00000010, 0x20404010,
                0x00400000, 0x20004000, 0x00404010, 0x00400000, 0x20000010, 0x00400010, 0x20004000, 0x20000000, 0x00004010,
                0x00000000, 0x00400010, 0x20004010, 0x00004000, 0x00404000, 0x20004010, 0x00000010, 0x20400010, 0x20400010,
                0x00000000, 0x00404010, 0x20404000, 0x00004010, 0x00404000, 0x20404000, 0x20000000, 0x20004000, 0x00000010,
                0x20400010, 0x00404000, 0x20404010, 0x00400000, 0x00004010, 0x20000010, 0x00400000, 0x20004000, 0x20000000,
                0x00004010, 0x20000010, 0x20404010, 0x00404000, 0x20400000, 0x00404010, 0x20404000, 0x00000000, 0x20400010,
                0x00000010, 0x00004000, 0x20400000, 0x00404010, 0x00004000, 0x00400010, 0x20004010, 0x00000000, 0x20404000,
                0x20000000, 0x00400010, 0x20004010 };

        private static readonly uint[] SP7 = { 0x00200000, 0x04200002, 0x04000802, 0x00000000, 0x00000800, 0x04000802, 0x00200802,
                0x04200800, 0x04200802, 0x00200000, 0x00000000, 0x04000002, 0x00000002, 0x04000000, 0x04200002, 0x00000802,
                0x04000800, 0x00200802, 0x00200002, 0x04000800, 0x04000002, 0x04200000, 0x04200800, 0x00200002, 0x04200000,
                0x00000800, 0x00000802, 0x04200802, 0x00200800, 0x00000002, 0x04000000, 0x00200800, 0x04000000, 0x00200800,
                0x00200000, 0x04000802, 0x04000802, 0x04200002, 0x04200002, 0x00000002, 0x00200002, 0x04000000, 0x04000800,
                0x00200000, 0x04200800, 0x00000802, 0x00200802, 0x04200800, 0x00000802, 0x04000002, 0x04200802, 0x04200000,
                0x00200800, 0x00000000, 0x00000002, 0x04200802, 0x00000000, 0x00200802, 0x04200000, 0x00000800, 0x04000002,
                0x04000800, 0x00000800, 0x00200002};

        private static readonly uint[] SP8 = { 0x10001040, 0x00001000, 0x00040000, 0x10041040, 0x10000000, 0x10001040, 0x00000040,
                0x10000000, 0x00040040, 0x10040000, 0x10041040, 0x00041000, 0x10041000, 0x00041040, 0x00001000, 0x00000040,
                0x10040000, 0x10000040, 0x10001000, 0x00001040, 0x00041000, 0x00040040, 0x10040040, 0x10041000, 0x00001040,
                0x00000000, 0x00000000, 0x10040040, 0x10000040, 0x10001000, 0x00041040, 0x00040000, 0x00041040, 0x00040000,
                0x10041000, 0x00001000, 0x00000040, 0x10040040, 0x00001000, 0x00041040, 0x10001000, 0x00000040, 0x10000040,
                0x10040000, 0x10040040, 0x10000000, 0x00040000, 0x10001040, 0x00000000, 0x10041040, 0x00040040, 0x10000040,
                0x10040000, 0x10001000, 0x10001040, 0x00000000, 0x10041040, 0x00041000, 0x00041000, 0x00001040, 0x00001040,
                0x00040040, 0x10000000, 0x10041000 };


        protected byte[] key;
        protected uint[] encKey;
        protected uint[] decKey;


        public DesKey(byte[] key)
        {
            this.key = key;
            this.encKey = generateWorkingKey(true, key, 0);
            this.decKey = generateWorkingKey(false, key, 0);
        }

        public uint getBlockSize()
        {
            return 8;
        }

        public void dispose()
        {
            uint i = 0;
            for (i = 0; i < encKey.Length; i++) { encKey[i] = 0; }
            for (i = 0; i < decKey.Length; i++) { decKey[i] = 0; }
            encKey = null;
            decKey = null;
            for (i = 0; i < key.Length; i++) { key[i] = 0; }
            key = null;
        }

        public byte[] encrypt(byte[] block, uint index = 0)
        {
            //if (block.Length % 8 != 0)
            //{
            //    byte[] newBlock = new byte[(block.Length / 8 + 1) * 8];
            //    for (int i = block.Length; i < newBlock.Length; ++i)
            //    {
            //        newBlock[i] = 0;
            //    }
            //    Array.Copy(block, newBlock, block.Length);
            //    block = newBlock;
            //}
            //for (uint i = index; i < block.Length; i += 8)
            //{
            //    desFunc(encKey, block, i, block, i);
            //}
            //return block;
            desFunc(encKey, block, index, block, index);
            return block;
        }

        public byte[] decrypt(byte[] block, uint index = 0)
        {
            //if (block.Length % 8 != 0)
            //{
            //    byte[] newBlock = new byte[(block.Length / 8 + 1) * 8];
            //    for (int i = block.Length; i < newBlock.Length; ++i)
            //    {
            //        newBlock[i] = 0;
            //    }
            //    Array.Copy(block, newBlock, block.Length);
            //    block = newBlock;
            //}
            //for (uint i = index; i < block.Length; i += 8)
            //{
            //    desFunc(decKey, block, i, block, i);
            //}
            desFunc(decKey, block, index, block, index);
            return block;
        }


        /**
         * generate an integer based working key based on our secret key and what we
         * processing we are planning to do.
         *
         * Acknowledgements for this routine go to James Gillogly & Phil Karn.
         */
        protected uint[] generateWorkingKey(bool encrypting, byte[] key, uint off)
        {
            uint[] newKey = new uint[32];
            bool[] pc1m = new bool[56], pcr = new bool[56];

            uint l;

            for (uint j = 0; j < 56; j++)
            {
                l = pc1[j];
                try {
                    pc1m[j] = ((key[off + (l >> 3)] & bytebit[l & 07]) != 0);
                }
                catch
                { }
                

            }

            for (uint i = 0; i < 16; i++)
            {
                uint m;
                uint n;

                if (encrypting)
                {
                    m = i << 1;
                }
                else
                {
                    m = (15 - i) << 1;
                }

                n = m + 1;
                newKey[m] = newKey[n] = 0;

                for (uint j = 0; j < 28; j++)
                {
                    l = j + totrot[i];
                    if (l < 28)
                    {
                        pcr[j] = pc1m[l];
                    }
                    else
                    {
                        pcr[j] = pc1m[l - 28];
                    }
                }

                for (uint j = 28; j < 56; j++)
                {
                    l = j + totrot[i];
                    if (l < 56)
                    {
                        pcr[j] = pc1m[l];
                    }
                    else
                    {
                        pcr[j] = pc1m[l - 28];
                    }
                }

                for (uint j = 0; j < 24; j++)
                {
                    if (pcr[pc2[j]])
                    {
                        newKey[m] |= bigbyte[j];
                    }

                    if (pcr[pc2[j + 24]])
                    {
                        newKey[n] |= bigbyte[j];
                    }
                }
            }

            //
            // store the processed key
            //
            for (uint i = 0; i != 32; i += 2)
            {
                uint i1;
                uint i2;

                i1 = newKey[i];
                i2 = newKey[i + 1];

                newKey[i] = ((i1 & 0x00fc0000) << 6) | ((i1 & 0x00000fc0) << 10) | ((i2 & 0x00fc0000) >> 10)
                        | ((i2 & 0x00000fc0) >> 6);

                newKey[i + 1] = ((i1 & 0x0003f000) << 12) | ((i1 & 0x0000003f) << 16) | ((i2 & 0x0003f000) >> 4)
                        | (i2 & 0x0000003f);
            }
            return newKey;
        }

        /**
         * the DES engine.
         */
        protected void desFunc(uint[] wKey, byte[] inp, uint inOff, byte[] outp, uint outOff)
        {
            uint work = 0;
            uint right = 0;
            uint left = 0;

            //处理数组长度不够8的情况
            //int[] shiftOffset = { 24, 16, 8, 0, 24, 16, 8, 0 };
            //for (int i = 0; i < 8; ++i)
            //{
            //    if (inOff + i >= inp.Length)
            //        break;

            //    if (i < 4)
            //    {
            //        if (left == 0)
            //        {
            //            left = ((uint)inp[inOff + i] & 0xff) << shiftOffset[i];
            //        }
            //        else
            //        {
            //            left |= ((uint)inp[inOff + i] & 0xff) << shiftOffset[i];
            //        }
            //    }
            //    else
            //    {
            //        if (right == 0)
            //        {
            //            right = ((uint)inp[inOff + i] & 0xff) << shiftOffset[i];
            //        }
            //        else
            //        {
            //            right |= ((uint)inp[inOff + i] & 0xff) << shiftOffset[i];
            //        }
            //    }
            //}

            //老的代码
            left = ((uint)inp[inOff + 0] & 0xff) << 24;
            left |= ((uint)inp[inOff + 1] & 0xff) << 16;
            left |= ((uint)inp[inOff + 2] & 0xff) << 8;
            left |= ((uint)inp[inOff + 3] & 0xff);

            right = ((uint)inp[inOff + 4] & 0xff) << 24;
            right |= ((uint)inp[inOff + 5] & 0xff) << 16;
            right |= ((uint)inp[inOff + 6] & 0xff) << 8;
            right |= ((uint)inp[inOff + 7] & 0xff);

            work = ((left >> 4) ^ right) & 0x0f0f0f0f;
            right ^= work;
            left ^= (work << 4);
            work = ((left >> 16) ^ right) & 0x0000ffff;
            right ^= work;
            left ^= (work << 16);
            work = ((right >> 2) ^ left) & 0x33333333;
            left ^= work;
            right ^= (work << 2);
            work = ((right >> 8) ^ left) & 0x00ff00ff;
            left ^= work;
            right ^= (work << 8);
            right = ((right << 1) | ((right >> 31) & 1)) & 0xffffffff;
            work = (left ^ right) & 0xaaaaaaaa;
            left ^= work;
            right ^= work;
            left = ((left << 1) | ((left >> 31) & 1)) & 0xffffffff;

            for (uint round = 0; round < 8; round++)
            {
                uint fval;

                work = (right << 28) | (right >> 4);
                work ^= wKey[round * 4 + 0];
                //work = work;
                fval = SP7[work & 0x3f];
                fval |= SP5[(work >> 8) & 0x3f];
                fval |= SP3[(work >> 16) & 0x3f];
                fval |= SP1[(work >> 24) & 0x3f];
                work = right ^ wKey[round * 4 + 1];
                fval |= SP8[work & 0x3f];
                fval |= SP6[(work >> 8) & 0x3f];
                fval |= SP4[(work >> 16) & 0x3f];
                fval |= SP2[(work >> 24) & 0x3f];
                left ^= fval;
                work = (left << 28) | (left >> 4);
                work ^= wKey[round * 4 + 2];
                fval = SP7[work & 0x3f];
                fval |= SP5[(work >> 8) & 0x3f];
                fval |= SP3[(work >> 16) & 0x3f];
                fval |= SP1[(work >> 24) & 0x3f];
                work = left ^ wKey[round * 4 + 3];
                fval |= SP8[work & 0x3f];
                fval |= SP6[(work >> 8) & 0x3f];
                fval |= SP4[(work >> 16) & 0x3f];
                fval |= SP2[(work >> 24) & 0x3f];
                right ^= fval;
            }

            right = (right << 31) | (right >> 1);
            work = (left ^ right) & 0xaaaaaaaa;
            left ^= work;
            right ^= work;
            left = (left << 31) | (left >> 1);
            work = ((left >> 8) ^ right) & 0x00ff00ff;
            right ^= work;
            left ^= (work << 8);
            work = ((left >> 2) ^ right) & 0x33333333;
            right ^= work;
            left ^= (work << 2);
            work = ((right >> 16) ^ left) & 0x0000ffff;
            left ^= work;
            right ^= (work << 16);
            work = ((right >> 4) ^ left) & 0x0f0f0f0f;
            left ^= work;
            right ^= (work << 4);

            //处理数组长度不够8的情况
            //for (uint i = 0; i < 8; ++i)
            //{
            //    if (outOff + i >= outp.Length)
            //        break;
            //    if (i < 4)
            //    {
            //        outp[outOff + i] = (byte)((right >> shiftOffset[i]) & 0xff);
            //    }
            //    else
            //    {
            //        outp[outOff + i] = (byte)((left >> shiftOffset[i]) & 0xff);
            //    }

            //}

            outp[outOff + 0] = (byte)((right >> 24) & 0xff);
            outp[outOff + 1] = (byte)((right >> 16) & 0xff);
            outp[outOff + 2] = (byte)((right >> 8) & 0xff);
            outp[outOff + 3] = (byte)(right & 0xff);
            outp[outOff + 4] = (byte)((left >> 24) & 0xff);
            outp[outOff + 5] = (byte)((left >> 16) & 0xff);
            outp[outOff + 6] = (byte)((left >> 8) & 0xff);
            outp[outOff + 7] = (byte)(left & 0xff);
        }

        public string toString()
        {
            return "des";
        }
    }
}