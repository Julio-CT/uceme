import { ReactElement } from 'react';

type BlogItem = {
  id: string;
  title?: string;
  imageSrc: string;
  text: string;
  altText: string | ReactElement | ReactElement[];
  caption: string;
  link: string;
  date: string;
  slug?: string;
  seoTitle?: string;
  metaDescription?: string;
  featuredImage?: string;
};

export default BlogItem;
