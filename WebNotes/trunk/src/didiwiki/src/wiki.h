#ifndef _HAVE_WIKI_HEADER
#define _HAVE_WIKI_HEADER

void
wiki_show_header(HttpResponse *res, char *page_title, int want_edit);

void
wiki_show_footer(HttpResponse *res);

void
wiki_print_data_as_html(HttpResponse *res, char *raw_page_data);

int
wiki_init(void);


#endif
