�}�  �   ����W2���!	��5N~(�^��o)i���H+�[SX��ѤF�5����b�m	�����ҭ[��iPf��;�';~̕rǿ�N�n��')�7��#��0�F)� ��f*�N�������E�.Nd����G�['����Ʋ��X?,��I�(���;�֕��yP/̕,UX��)�YW�p~��!��5a��"����E���A���M�#�$��C�dg��P�x3������>��Y�2Mޯ�L�?�~w��G��:���4�$Jj5�)�m�3� ���-O�$
a�O�uU���T���5��e!U0rZ����&�eX��Ɗ�MR��d�2H�l�<�$	N���R	 f�A�ði>�u�Ѱf���#$7,�p"���{��::v��������=�aW�T�n;����ިL�RH��\͙���g=i����ڙ���]��=�^h/G<<�Q�G�9�V#۬��7�o�|�2p��jLd=l�/������R���o~=W_#�Ҷ��ē��6����`���i<c�z�lM�0Y�����BN�)W~0NE�q"k~(��P��HU�c`t��\^�d�)=�'�'s�p���%��mV�c8�z���˵8~����v�ʥ�V�
�׷2��u�A�x	+��6RE=ݯfD.�^C^?%�N�ӓ$( ��髭iԉA�bG	��_o����ex~�|�1�H��U\>$�hu#_@��S�"�}=���KO�������	�)��AY3�Q˻�������9�쮃�>�z
c�$�Y��	�$�>y���+�Q8L�x[������H���F��e��}f��f�C�>�9���{��z�?W��O~��3�/�X�ф�+��q�S��!79��{! ��Q�MkJ����^��xn�\}Ѵ�Z6-���y�.'�R��K��sF��j>��rh�ug(�\�6��wvQ���A�s�[_�8�WF)z�S���݊R@�{
        public static UnSubResponse Unmarshall(UnmarshallerContext context)
        {
			UnSubResponse unSubResponse = new UnSubResponse();

			unSubResponse.HttpResponse = context.HttpResponse;
			unSubResponse.RequestId = context.StringValue("UnSub.RequestId");
			unSubResponse.Success = context.BooleanValue("UnSub.Success");
			unSubResponse.ErrorMessage = context.StringValue("UnSub.ErrorMessage");
        
			return unSubResponse;
        }
    }
}