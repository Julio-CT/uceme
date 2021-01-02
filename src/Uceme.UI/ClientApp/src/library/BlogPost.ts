type BlogPost = {
  id: string;
  title?: string;
  src: string;
  altText: JSX.Element | JSX.Element[];
  caption: string;
  link: string;
  date: string;
  slug?: string;
  fields?: any;
  seoTitle?: string;
  metaDescription?: string;
  featuredImage?: string;
};

export default BlogPost;
