type BlogPostResponse = {
  idBlog: string;
  titulo: string;
  foto: string;
  texto: string;
  fecha: string;
  slug?: string;
  seoTitle: string;
  metaDescription?: string;
  featuredImage?: string;
};

export default BlogPostResponse;
