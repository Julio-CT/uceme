type BlogPost = {
  id: string;
  title?: string;
  imageSrc: string;
  text: string;
  altText: JSX.Element | JSX.Element[];
  caption: string;
  link: string;
  date: string;
  slug?: string;
  seoTitle?: string;
  metaDescription?: string;
  featuredImage?: string;
};

export default BlogPost;
