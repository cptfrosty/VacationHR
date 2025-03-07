PGDMP                      }         
   VacationHR    16.4    16.4 (    �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    17396 
   VacationHR    DATABASE     �   CREATE DATABASE "VacationHR" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';
    DROP DATABASE "VacationHR";
                postgres    false                        3079    17450    pgcrypto 	   EXTENSION     <   CREATE EXTENSION IF NOT EXISTS pgcrypto WITH SCHEMA public;
    DROP EXTENSION pgcrypto;
                   false                        0    0    EXTENSION pgcrypto    COMMENT     <   COMMENT ON EXTENSION pgcrypto IS 'cryptographic functions';
                        false    2            �            1259    17398    roles    TABLE     f   CREATE TABLE public.roles (
    id integer NOT NULL,
    role_name character varying(255) NOT NULL
);
    DROP TABLE public.roles;
       public         heap    postgres    false            �            1259    17397    roles_id_seq    SEQUENCE     �   CREATE SEQUENCE public.roles_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 #   DROP SEQUENCE public.roles_id_seq;
       public          postgres    false    217                       0    0    roles_id_seq    SEQUENCE OWNED BY     =   ALTER SEQUENCE public.roles_id_seq OWNED BY public.roles.id;
          public          postgres    false    216            �            1259    17407    users    TABLE     �  CREATE TABLE public.users (
    id integer NOT NULL,
    first_name character varying(255) NOT NULL,
    last_name character varying(255) NOT NULL,
    middle_name character varying(255),
    department character varying(255),
    "position" character varying(255),
    email character varying(255) NOT NULL,
    password_hash character varying(255) NOT NULL,
    role_id integer NOT NULL,
    count_vacation_days integer NOT NULL
);
    DROP TABLE public.users;
       public         heap    postgres    false            �            1259    17406    users_id_seq    SEQUENCE     �   CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 #   DROP SEQUENCE public.users_id_seq;
       public          postgres    false    219                       0    0    users_id_seq    SEQUENCE OWNED BY     =   ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;
          public          postgres    false    218            �            1259    17423    vacation_request_statuses    TABLE     |   CREATE TABLE public.vacation_request_statuses (
    id integer NOT NULL,
    status_name character varying(255) NOT NULL
);
 -   DROP TABLE public.vacation_request_statuses;
       public         heap    postgres    false            �            1259    17422     vacation_request_statuses_id_seq    SEQUENCE     �   CREATE SEQUENCE public.vacation_request_statuses_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 7   DROP SEQUENCE public.vacation_request_statuses_id_seq;
       public          postgres    false    221                       0    0     vacation_request_statuses_id_seq    SEQUENCE OWNED BY     e   ALTER SEQUENCE public.vacation_request_statuses_id_seq OWNED BY public.vacation_request_statuses.id;
          public          postgres    false    220            �            1259    17432    vacation_requests    TABLE       CREATE TABLE public.vacation_requests (
    id integer NOT NULL,
    user_id integer NOT NULL,
    start_date date NOT NULL,
    end_date date NOT NULL,
    reason text,
    status_id integer NOT NULL,
    request_date date NOT NULL,
    manager_comment text
);
 %   DROP TABLE public.vacation_requests;
       public         heap    postgres    false            �            1259    17431    vacation_requests_id_seq    SEQUENCE     �   CREATE SEQUENCE public.vacation_requests_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 /   DROP SEQUENCE public.vacation_requests_id_seq;
       public          postgres    false    223                       0    0    vacation_requests_id_seq    SEQUENCE OWNED BY     U   ALTER SEQUENCE public.vacation_requests_id_seq OWNED BY public.vacation_requests.id;
          public          postgres    false    222            N           2604    17401    roles id    DEFAULT     d   ALTER TABLE ONLY public.roles ALTER COLUMN id SET DEFAULT nextval('public.roles_id_seq'::regclass);
 7   ALTER TABLE public.roles ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    217    216    217            O           2604    17410    users id    DEFAULT     d   ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);
 7   ALTER TABLE public.users ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    219    218    219            P           2604    17426    vacation_request_statuses id    DEFAULT     �   ALTER TABLE ONLY public.vacation_request_statuses ALTER COLUMN id SET DEFAULT nextval('public.vacation_request_statuses_id_seq'::regclass);
 K   ALTER TABLE public.vacation_request_statuses ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    220    221    221            Q           2604    17435    vacation_requests id    DEFAULT     |   ALTER TABLE ONLY public.vacation_requests ALTER COLUMN id SET DEFAULT nextval('public.vacation_requests_id_seq'::regclass);
 C   ALTER TABLE public.vacation_requests ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    222    223    223            �          0    17398    roles 
   TABLE DATA           .   COPY public.roles (id, role_name) FROM stdin;
    public          postgres    false    217   �.       �          0    17407    users 
   TABLE DATA           �   COPY public.users (id, first_name, last_name, middle_name, department, "position", email, password_hash, role_id, count_vacation_days) FROM stdin;
    public          postgres    false    219   #/       �          0    17423    vacation_request_statuses 
   TABLE DATA           D   COPY public.vacation_request_statuses (id, status_name) FROM stdin;
    public          postgres    false    221   4       �          0    17432    vacation_requests 
   TABLE DATA           �   COPY public.vacation_requests (id, user_id, start_date, end_date, reason, status_id, request_date, manager_comment) FROM stdin;
    public          postgres    false    223   �4                  0    0    roles_id_seq    SEQUENCE SET     :   SELECT pg_catalog.setval('public.roles_id_seq', 3, true);
          public          postgres    false    216                       0    0    users_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('public.users_id_seq', 22, true);
          public          postgres    false    218                       0    0     vacation_request_statuses_id_seq    SEQUENCE SET     N   SELECT pg_catalog.setval('public.vacation_request_statuses_id_seq', 5, true);
          public          postgres    false    220                       0    0    vacation_requests_id_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('public.vacation_requests_id_seq', 4, true);
          public          postgres    false    222            S           2606    17403    roles roles_pkey 
   CONSTRAINT     N   ALTER TABLE ONLY public.roles
    ADD CONSTRAINT roles_pkey PRIMARY KEY (id);
 :   ALTER TABLE ONLY public.roles DROP CONSTRAINT roles_pkey;
       public            postgres    false    217            U           2606    17405    roles roles_role_name_key 
   CONSTRAINT     Y   ALTER TABLE ONLY public.roles
    ADD CONSTRAINT roles_role_name_key UNIQUE (role_name);
 C   ALTER TABLE ONLY public.roles DROP CONSTRAINT roles_role_name_key;
       public            postgres    false    217            W           2606    17416    users users_email_key 
   CONSTRAINT     Q   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_email_key UNIQUE (email);
 ?   ALTER TABLE ONLY public.users DROP CONSTRAINT users_email_key;
       public            postgres    false    219            Y           2606    17414    users users_pkey 
   CONSTRAINT     N   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);
 :   ALTER TABLE ONLY public.users DROP CONSTRAINT users_pkey;
       public            postgres    false    219            [           2606    17428 8   vacation_request_statuses vacation_request_statuses_pkey 
   CONSTRAINT     v   ALTER TABLE ONLY public.vacation_request_statuses
    ADD CONSTRAINT vacation_request_statuses_pkey PRIMARY KEY (id);
 b   ALTER TABLE ONLY public.vacation_request_statuses DROP CONSTRAINT vacation_request_statuses_pkey;
       public            postgres    false    221            ]           2606    17430 C   vacation_request_statuses vacation_request_statuses_status_name_key 
   CONSTRAINT     �   ALTER TABLE ONLY public.vacation_request_statuses
    ADD CONSTRAINT vacation_request_statuses_status_name_key UNIQUE (status_name);
 m   ALTER TABLE ONLY public.vacation_request_statuses DROP CONSTRAINT vacation_request_statuses_status_name_key;
       public            postgres    false    221            _           2606    17439 (   vacation_requests vacation_requests_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY public.vacation_requests
    ADD CONSTRAINT vacation_requests_pkey PRIMARY KEY (id);
 R   ALTER TABLE ONLY public.vacation_requests DROP CONSTRAINT vacation_requests_pkey;
       public            postgres    false    223            `           2606    17417    users users_role_id_fkey    FK CONSTRAINT     w   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_role_id_fkey FOREIGN KEY (role_id) REFERENCES public.roles(id);
 B   ALTER TABLE ONLY public.users DROP CONSTRAINT users_role_id_fkey;
       public          postgres    false    217    219    4691            a           2606    17445 2   vacation_requests vacation_requests_status_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.vacation_requests
    ADD CONSTRAINT vacation_requests_status_id_fkey FOREIGN KEY (status_id) REFERENCES public.vacation_request_statuses(id);
 \   ALTER TABLE ONLY public.vacation_requests DROP CONSTRAINT vacation_requests_status_id_fkey;
       public          postgres    false    223    4699    221            b           2606    17440 0   vacation_requests vacation_requests_user_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.vacation_requests
    ADD CONSTRAINT vacation_requests_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id);
 Z   ALTER TABLE ONLY public.vacation_requests DROP CONSTRAINT vacation_requests_user_id_fkey;
       public          postgres    false    223    4697    219            �   C   x�3估�b�]�]��[.��ta��{��8=���9/,���b����-�^�qaW� � �      �   �  x��V�o�V~v�
�:0��6�'�Hl4)2`����扦�i�VU�VmݦL{�h��4iؿ`�G;��ݴ��`�{|��;�9�%�P���[���3������wkxn�y�-(�w���w�[l��/ay�ޭ�5��"|������C�u�?��:�||KyOa����/C�q�!����k��D�r��_�U�
�l��ܣ����B��t��4�X�;�ɢZ�LRa#|�j��r�ϗ$��F=^��T���;�彀��1T�������`l�b\�����P��xFb� *ཆ�-�甩6ն�X�&�0'2�xV��Few`2�6��7o�	�Z��L�(f��l_he&1=��O�B��~ �$�99r�k��~O�3@��Z���!�����s�%n��V���	��X�d�P��	7���n�a��B�xO�K�o��iڶR	V�p\��v*9^���9MU����Ť�@֘�Ś>�1���r���b�n��'��KB�5�a�����.���,���P��t�}ے��8��9Bn^l���Z*��n�'U�WR,*���y�m���x�Ո��ƴ�9�����ߡ:����l��{����K��}t��k���&�iN
w#�
�u�j�Z�ls����^(	)��r=e�|��(��A% W��N����$D�}��jK����1�#N�SK�18�ɸ���i���Z/��k�#�Q3Q�O4��;%4�R��3���P@57 �Q���2���_�����IzJ~�J�q��ҽ��b�h���Ep+�0gkr׭M�n5P+�ل_��|5;IK��[���db�~��TR���v��v���D;���ZR'�Ob-������(�*@	4���lM����a=�z���3r�hj�0�mwd%5]:��jk�74S�s4gr����!ݯ�"Gv5����>�34n�W���O�������"Lp��u�?�9��*����~{���'��٫\^��j�c�Ų:\1�T��)e9���d���+$�Ӯ<�)������~�2�]��}�=$ʱ%E�W�w�PZ��d�ޒ��A��Fx�%5S���nX�(�� &�hU4l�d�jEL;�Q�\1r�zlyv�K��0�-�z0��H�ug���H軛�Z� �����ɞ0ӹ�~�:�FN��6'̚���5�E.�d�i�ED&ӰU*��|�����e��      �   q   x�e���@E�;UP�	��X��p�fbl`�,l4PÛ��n&�y��Lx�01��Iv�)�=V~��K�*<��*�Yx��N҅?|uu��9Г�˼%���f�������EZ�      �   {   x�3�4��4202�50"8�Д����.l��@����;.�T���bӅ��/6^��	VT�aʩ�e�R�,cNCj�e5�������_�p��y��@��bD;P���+F��� ��r�     